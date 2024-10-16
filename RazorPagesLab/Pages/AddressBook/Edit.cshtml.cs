using System;
using System.Data.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesLab.Pages.AddressBook;

public class EditModel : PageModel
{
	private readonly IMediator _mediator;
	private readonly IRepo<AddressBookEntry> _repo;

	public EditModel(IRepo<AddressBookEntry> repo, IMediator mediator)
	{
		_repo = repo;
		_mediator = mediator;
	}

	[BindProperty]
	public UpdateAddressRequest UpdateAddressRequest { get; set; }

	public void OnGet(Guid id)
	{
		// Find the entry and set fields, if not found call NotFound()

		var entry = _repo.Find(new EntryByIdSpecification(id));

		if(entry.Count != 1)
			throw new Exception("Failed to pull 1 entry in OnGet of Edit");

		if(entry[0] != null) {
			UpdateAddressRequest = new UpdateAddressRequest {
											Id = entry[0].Id,
											Line1 = entry[0].Line1,
											Line2 = entry[0].Line2,
											City = entry[0].City,
											State = entry[0].State,
											PostalCode = entry[0].PostalCode
			};
	
		} else {
			NotFound();
		}
	}

	public ActionResult OnPost()
	{
		//Use mediator to send a "command" to update the address book entry, redirect to entry list.
		if (ModelState.IsValid)
		{
			_ =  _mediator.Send(UpdateAddressRequest);
			return RedirectToPage("Index");
		}

		return Page();
	}
}