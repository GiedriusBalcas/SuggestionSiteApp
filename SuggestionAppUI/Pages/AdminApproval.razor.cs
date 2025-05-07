namespace SuggestionAppUI.Pages;
public partial class AdminApproval
{
   private List<SuggestionModel> _submissions;
   private SuggestionModel _editingModel;
   private string _currentEditingTitle = "";
   private string _editedTitle = "";
   private string _currentEditingDescription = "";
   private string _editedDescription = "";

   protected async override Task OnInitializedAsync()
   {
      _submissions = await suggestionData.GetAllSuggestionsWaitingForApproval();
   }

   private async Task ApproveSubmission(SuggestionModel submission)
   {
      submission.ApprovedForRelease = true;
      _submissions.Remove(submission);
      await suggestionData.UpdateSuggestion(submission);
   }

   private async Task RejectSubmission(SuggestionModel submission)
   {
      submission.Rejected = true;
      _submissions.Remove(submission);
      await suggestionData.UpdateSuggestion(submission);
   }
   private void EditTitle(SuggestionModel model)
   {
      _editingModel = model;
      _editedTitle = model.Suggestion;
      _currentEditingTitle = model.Id;
      _currentEditingDescription = "";
   }

   private async Task SaveTitle(SuggestionModel model)
   {
      _currentEditingTitle = string.Empty;
      model.Suggestion = _editedTitle;
      await suggestionData.UpdateSuggestion(model);
   }

   private void EditDescription(SuggestionModel model)
   {
      _editingModel = model;
      _editedDescription = model.Description;
      _currentEditingDescription = model.Id;
      _currentEditingTitle = "";
   }

   private async Task SaveDescription(SuggestionModel model)
   {
      _currentEditingDescription = string.Empty;
      model.Description = _editedDescription;
      await suggestionData.UpdateSuggestion(model);
   }

   private void ClosePage()
   {
      navManager.NavigateTo("/");
   }
}