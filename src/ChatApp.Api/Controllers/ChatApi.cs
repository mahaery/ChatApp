using ChatApp.Api.Models;
using ChatApp.Application.Common.Interfaces.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class ChatApi : ControllerBase
{
    private readonly IChatSessionQueueService _chatSessionQueueService;
    public ChatApi(IChatSessionQueueService chatSessionQueueService)
    {
        _chatSessionQueueService = chatSessionQueueService;
    }

    [HttpPost]
    public async Task<IActionResult> StartChat(ChatRequest request)
    {
        try
        {
            var queuedChatId = await _chatSessionQueueService.CreateChatSession(request.UserId, true);

            if (queuedChatId.Equals(Guid.Empty))
                return BadRequest("Error occurred while handling your request, please try again.");

            return Ok(queuedChatId);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}