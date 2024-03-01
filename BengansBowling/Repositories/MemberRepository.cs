using BengansBowling.Models;
using BengansBowling.Observers;
using BengansBowling.Singletons;
using BengansBowling.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BengansBowling.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private const string FilePath = "Members.json";
        private List<Member> _members;
        private readonly ApplicationEventPublisher _eventPublisher;
        

        public MemberRepository() {
            _members = FileStorageHelper.ReadFromJsonFile<List<Member>>(FilePath) ?? new List<Member>();

            _eventPublisher = new ApplicationEventPublisher();
            var loggerObserver = new LoggerObserver();
            _eventPublisher.Attach(loggerObserver);
        }

        public IEnumerable<Member> GetAll() {
            return _members;
        }

        public Member GetById(int id) {
            return _members.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Member member) {
            _members.Add(member);
            SaveChanges();
            _eventPublisher.Notify($"Member added: {member.Name}");
        }

        public void Update(Member member) {
            var index = _members.FindIndex(m => m.Id == member.Id);
            if (index != -1) {
                _members[index] = member;
                SaveChanges();
                _eventPublisher.Notify($"Member updated: {member.Name}");
            }
        }

        public void Delete(int id) {
            var member = GetById(id);
            if (member != null) {
                _members.Remove(member);
                SaveChanges();
                _eventPublisher.Notify($"Member deleted: {member.Name}");
            }
        }

        private void SaveChanges() {
            FileStorageHelper.WriteToJsonFile(FilePath, _members);
        }
    }

}
