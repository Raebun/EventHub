using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class FavoriteController : Controller
{
	private readonly IFavoriteService _favoriteService;

	public FavoriteController(IFavoriteService favoriteService)
	{
		_favoriteService = favoriteService;
	}

	[HttpGet("{userId}")]
	public async Task<ActionResult<List<Favorite>>> GetFavoritesForUser(Guid userId)
	{
		var favorite = await _favoriteService.GetFavoritesForUserAsync(userId);
		return Ok(favorite);
	}

	[HttpPost]
	public async Task<ActionResult> AddFavorite([FromBody] AddFavoriteModel request)
	{
		try
		{
			await _favoriteService.AddFavoriteAsync(request.UserId, request.EventId);
			return Ok();
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete]
	public async Task<ActionResult> RemoveFavorite([FromBody] AddFavoriteModel request)
	{
		try
		{
			await _favoriteService.RemoveFavoriteAsync(request.UserId, request.EventId);
			return Ok();
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}