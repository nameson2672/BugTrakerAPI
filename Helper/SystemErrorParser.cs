using Microsoft.AspNetCore.Identity;

namespace BugTrakerAPI.Helper
{
    public class SystemErrorParser
    {
        public List<string> IdentityErrorParser(IEnumerable<IdentityError> error)
        {
            List<string> errors = new List<string>();
            foreach (var errorItem in error)
            {
                errors.Add(errorItem.Description);


            }
            /*for(int i = 0; i < error.Count; i++)
            {
                errors.Add(error[i].Description);
            }*/
            return errors;
        }
    }
}
