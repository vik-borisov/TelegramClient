namespace TelegramClient.Core.ApiServies.Interfaces
{
    using System.Threading.Tasks;

    using OpenTl.Schema;

    public interface ISenderService
    {
        Task<TResult> SendRequestAsync<TResult>(IRequest<TResult> methodToExecute);
    }
}