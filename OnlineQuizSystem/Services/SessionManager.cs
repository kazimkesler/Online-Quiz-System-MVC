using Newtonsoft.Json;
using OnlineQuizSystem.Models;
using OnlineQuizSystem.Models.ViewModels;

namespace OnlineQuizSystem.Services
{
    public class SessionManager
    {
        private readonly ISession session;
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext!.Session;
        }
        public SessionManager(ISession session)
        {
            this.session = session;
        }
        public void SetObject(string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public T GetObject<T>(string key)
        {
            string value = session.GetString(key)!;
            return value is null ? default(T)! : JsonConvert.DeserializeObject<T>(value)!;
        }
        public void Clear()
        {
            session.Clear();
        }
        public bool? State
        {
            get => GetObject<bool?>(nameof(State));
            set => SetObject(nameof(State), value!);
        }
        public bool? Success
        {
            get => GetObject<bool?>(nameof(Success));
            set => SetObject(nameof(Success), value!);
        }
        public int? Category
        {
            get => GetObject<int?>(nameof(Category));
            set
            {
                SetObject(nameof(Category), value!);
            }
        }
        public DateTime Start
        {
            get => GetObject<DateTime>(nameof(Start));
            set => SetObject(nameof(Start), value);
        }
        public List<UserAnswerVM> UserAnswers
        {
            get => GetObject<List<UserAnswerVM>>(nameof(UserAnswers));
            set => SetObject(nameof(UserAnswers), value);
        }
        public void UpdateAnswer(UserAnswerVM model)
        {
            var userAnswers = UserAnswers;
            for (int i = 0; i < userAnswers.Count; i++)
                if (userAnswers[i].QuestionId == model.QuestionId)
                {
                    userAnswers[i] = model;
                    break;
                }
            UserAnswers = userAnswers;
        }
        public UserAnswerVM CurrentUserAnswer { get => UserAnswers[(int)Category! - 1]; }
    }
}
