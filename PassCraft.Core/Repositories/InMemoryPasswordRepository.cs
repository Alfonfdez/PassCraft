using PassCraft.Core.Contracts;

namespace PassCraft.Core.Repositories
{
    /// <summary>
    /// An in-memory runtime implementation of the password repository 
    /// using a standard collection list.
    /// </summary>
    public class InMemoryPasswordRepository : IPasswordRepository
    {
        private readonly List<string> _storage = new List<string>();

        public void Save(string password)
        {
            _storage.Add(password);
        }

        public IReadOnlyList<string> GetAll()
        {
            return _storage.AsReadOnly();
        }

        public void RemoveOldest()
        {
            if (_storage.Count > 0)
            {
                _storage.RemoveAt(0);
            }
        }
    }
}
