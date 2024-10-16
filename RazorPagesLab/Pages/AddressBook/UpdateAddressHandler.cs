using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RazorPagesLab.Pages.AddressBook;

public class UpdateAddressHandler: IRequestHandler<UpdateAddressRequest>
{
	private readonly IRepo<AddressBookEntry> _repo;

	public UpdateAddressHandler(IRepo<AddressBookEntry> repo)
	{
		_repo = repo;
	}

	public Task<Unit> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
	{

        //From repo we find the entry, update the result with new data, then update it back in the repo

        var result = _repo.Find(new EntryByIdSpecification(request.Id));

        if(result.Count != 1)
            throw new Exception("Update count pulled more than 1 entry");

		result[0].Update(request.Line1, request.Line2, request.City, request.State,
            request.PostalCode);

        _repo.Update(result[0]);

        return null;
	}
}