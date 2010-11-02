namespace Admin
{
    using System.Collections;
    using System.Web.Services;
    using BlogEngine.Core.Json;

    public partial class Trash : System.Web.UI.Page
    {
        /// <summary>
        /// Number of items in the list
        /// </summary>
        protected static int TrashCounter { get; set; }

        [WebMethod]
        public static IEnumerable LoadTrash(int pageSize, int page)
        {
            var trashList = JsonTrashList.GetTrash(TrashType.All, pageSize, page);
            TrashCounter = trashList.Count;
            return trashList;
        }

        [WebMethod]
        public static JsonResponse ProcessTrash(string action, string[] vals)
        {
            return JsonTrashList.Process(action, vals);
        }
    }
}