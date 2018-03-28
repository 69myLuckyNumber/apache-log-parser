using System.Threading.Tasks;

namespace ApacheLogParser.Core.Abstract
{
    public interface IUnitOfWork
    {
         Task Commit();
    }
}