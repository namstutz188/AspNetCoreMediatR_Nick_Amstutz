using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class DeleteModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

    public Guid Id {get; set;}
    public string Line1 {get; set;}
    
    public string Line2 {get; set;}
    
    public string City {get; set;}
    
    public string State {get; set;}
    
    public string PostalCode {get; set;}

	public DeleteModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	public void OnGet(Guid id)
	{
        // Find the entry and set fields, if not found call NotFound()

		var entry = _repo.Find(new EntryByIdSpecification(id));

        if(entry.Count != 1)
            throw new Exception("Failed to pull 1 entry for Delete OnGet");

		if(entry[0] != null) {
            Id = entry[0].Id;
            Line1 = entry[0].Line1;
			Line2 = entry[0].Line2;
			City = entry[0].City;
			State = entry[0].State;
			PostalCode = entry[0].PostalCode;
		} else {
			NotFound();
		}
	}

	public ActionResult OnPost(Guid id)
	{
        //Find the unique item to remove and do so.

		if(_repo.Find(new EntryByIdSpecification(id)).Count != 1) {
		    throw new Exception($"Failure to find one item to remove: {id}");
		} else {
		    var entity = _repo.Find(new EntryByIdSpecification(id))[0];
		     _repo.Remove(entity);
		}
		return RedirectToPage("Index");

	}
}