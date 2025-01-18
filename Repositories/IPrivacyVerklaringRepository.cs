using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IPrivacyVerklaringRepository
    {
        public PrivacyVerklaring GetLatestPrivacyVerklaring();

        public void Add(PrivacyVerklaring privacyVerklaring);

        IQueryable<PrivacyVerklaring> GetAllPrivacyVerklaringen();

    }
}
