namespace SuggestionAppUI.Pages;
public partial class Profile
{
   private UserModel _loggedInUser;
   private List<SuggestionModel> _submissions;
   private List<SuggestionModel> _approvedSubmissions;
   private List<SuggestionModel> _archivedSubmissions;
   private List<SuggestionModel> _pendingSubmissions;
   private List<SuggestionModel> _rejectedSubmissions;

   protected async override Task OnInitializedAsync()
   {
      _loggedInUser = await authProvider.GetUserFromAuth(userData);

      var results = await suggestionData.GetUsersSuggestions(_loggedInUser.Id);

      if (_loggedInUser is not null && results is not null)
      {
         _submissions = results.OrderByDescending(s => s.DateCreated).ToList();
         _approvedSubmissions = _submissions.Where(s => s.ApprovedForRelease && !s.Archived && !s.Rejected).ToList();
         _archivedSubmissions = _submissions.Where(s => s.Archived && !s.Rejected).ToList();
         _pendingSubmissions = _submissions.Where(s => !s.ApprovedForRelease && !s.Archived && !s.Rejected).ToList();
         _rejectedSubmissions = _submissions.Where(s => s.Rejected).ToList();
      }
   }

   private void ClosePage()
   {
      navManager.NavigateTo("/");
   }
}