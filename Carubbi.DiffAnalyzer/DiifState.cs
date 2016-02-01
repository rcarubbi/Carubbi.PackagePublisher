
namespace Carubbi.DiffAnalyzer
{
    public enum DiffState : int
    {
        Unknow = 1,
        NotChanged= 2,
        Modified = 4,
        Added = 8,
        Deleted = 16
    }
}
