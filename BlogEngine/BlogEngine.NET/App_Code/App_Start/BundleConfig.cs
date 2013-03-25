using System.Web.Optimization;

/// <summary>
/// Summary description for BundleConfig
/// </summary>
public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        // for anonymous users
        bundles.Add(new StyleBundle("~/Content/Auto/css").Include(
            "~/Content/Auto/*.css")
        );
        bundles.Add(new ScriptBundle("~/Scripts/Auto/js").Include(
            "~/Scripts/Auto/*.js")
        );

        // for authenticated users
        bundles.Add(new StyleBundle("~/Content/Auto/cssauth").Include(
            "~/Content/Auto/*.css",
            "~/Modules/QuickNotes/Qnotes.css")
        );  
        bundles.Add(new ScriptBundle("~/Scripts/Auto/jsauth").Include(
            "~/Scripts/Auto/*.js")
        );

        // administration
        bundles.Add(new StyleBundle("~/admin/css").Include(
            "~/admin/style.css",
            "~/admin/colorbox.css",
            "~/admin/tipsy.css")
        );
        bundles.Add(new ScriptBundle("~/admin/js").Include(
            "~/Scripts/jQuery/*.js",
            "~/admin/admin.js")
        );

        // syntax highlighter 
        var shRoot = "~/editors/tiny_mce_3_5_8/plugins/syntaxhighlighter/";
        bundles.Add(new ScriptBundle("~/Scripts/highlighter").Include(
            shRoot + "scripts/XRegExp.js",
            shRoot + "scripts/shCore.js",
            shRoot + "scripts/shAutoloader.js",
            shRoot + "shActivator.js")
        );
    }
}