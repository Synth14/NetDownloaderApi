using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetDownloader.Entity.Interfaces;
using NetDownloader.Entity.Models;
using NetDownloader.Entity.Services;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;

namespace NetDownloader.Entity.Controllers
{
    public class AccountsController:Controller
    {
        private readonly IAccountsService _accountService;

        public AccountsController(IAccountsService accountService)
        {
            _accountService = accountService;
        }
        [ApiVersion("1")]
        [HttpPost("CreateAccount")]
        [SwaggerOperation(Summary = "CreateAccount", Description = "CreateAccount")]
        public async Task<IActionResult> CreateAccount(Accounts account)
        {
                var result = await _accountService.CreateAccountAsync(account);
                return Ok(result);
        }
        [ApiVersion("1")]
        [HttpGet("GetAllAccounts")]
        [SwaggerOperation(Summary = "Get all accounts", Description = "Get all accounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var allTags = await _accountService.GetAllAccountsAsync();
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpPatch("UpdateAccount")]
        [SwaggerOperation(Summary = "Update an account", Description = "Update an account")]
        public async Task<IActionResult> UpdateAccount(Accounts account)
        {
            var allTags = await _accountService.UpdateAccountAsync(account);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpGet("GetAccountById")]
        [SwaggerOperation(Summary = "GetAccountById", Description = "GetAccountById")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var allTags = await _accountService.GetAccountByIdAsync(id);
            return Ok(allTags);
        }
        [ApiVersion("1")]
        [HttpDelete("DeleteAccount")]
        [SwaggerOperation(Summary = "DeleteAccountId", Description = "DeleteAccountById")]
        public async Task<IActionResult> DeleteAccountById(int id)
        {
            var allTags = await _accountService.DeleteAccountByIdAsync(id);
            return Ok(allTags);
        }
    }
}
