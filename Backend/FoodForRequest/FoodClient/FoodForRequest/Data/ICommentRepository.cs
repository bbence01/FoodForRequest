using FoodForRequest.Models;

namespace FoodForRequest.Data
{
    public interface ICommentRepository
    {

        void Create(Comment comment);
        void Delete(Comment comment);
        void Delete(string id);
        IEnumerable<Comment> GetAll();
        Comment GetOne(string id);
        List<Comment> GetCommentssForRequest(string id);
    }
}
