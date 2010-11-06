﻿namespace Admin
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
        public static IEnumerable LoadTrash(string trashType)
        {
            var tType = TrashType.All;
            switch (trashType)
            {
                case "Post":
                    tType = TrashType.Post;
                    break;
                case "Page":
                    tType = TrashType.Page;
                    break;
                case "Comment":
                    tType = TrashType.Comment;
                    break;
                default:
                    break;
            }
            var trashList = JsonTrashList.GetTrash(tType);
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