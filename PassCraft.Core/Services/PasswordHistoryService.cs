using PassCraft.Core.Contracts;
using PassCraft.Core.Models;

namespace PassCraft.Core.Services
{
    public class PasswordHistoryService : IPasswordHistoryService
    {
        private readonly IPasswordRepository _repository;

        public PasswordHistoryService(IPasswordRepository repository)
        {
            _repository = repository;
        }

        public void AddPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return;

            var currentHistory = _repository.GetAll();

            while (currentHistory.Count >= Constants.Constants.SecuritySettings.MaxHistoryRecords)
            {
                _repository.RemoveOldest();
            }

            _repository.Save(password);
        }

        public List<PasswordItem> GetHistory()
        {
            var rawPasswords = _repository.GetAll();
            var formattedList = new List<PasswordItem>();
            int totalItems = rawPasswords.Count;

            for (int i = 0; i < totalItems; i++)
            {
                formattedList.Add(new PasswordItem
                {
                    Index = i + 1,
                    Password = rawPasswords[i]
                });
            }

            formattedList.Reverse();
            return formattedList;
        }
    }
}