
var pageSize = 10;

LoadView();

$(document).ready(function () {
   $('.editButton').live("click", function () { return EditRow(this); });
   $('.deleteButton').live("click", function () { return DeleteRow(this); });
   $('.loader').hide();
});

//-------------		EDITING

function EditRow(obj) {
   var row = $(obj).closest("tr");
   var revert = $(row).html();
   var button = '<div><input type="button" value="Save" class="saveButton btn" /> <a href="#" class="cancelButton">Cancel</a></div>';

   $('.editable', row).each(function () {
      var _this = $(this);
      var _thisHtml = _this.html();
      var txt = '<td><input id="' + _thisHtml + '" type=\"text\" class=\"txt200\" value=\"' + _thisHtml + '"/></td>';
      _this.after(txt).remove();
  });

  var delCel = $(row).find('.deleteButton').closest("td");
  $(delCel).remove();

  var editCel = $(row).find('.editButton').closest("td");
  $(editCel).attr('colspan', '2');
  $(row).find('.editButton').replaceWith(button);

   var cancelButton = $('.cancelButton');
   var saveButton = $('.saveButton');

   cancelButton.unbind('click');
   saveButton.unbind('click');

   cancelButton.bind("click", function () { CancelChanges(this, revert); });
   saveButton.bind("click", function () { SaveChanges(this, revert); });

   return false;
}

function SaveChanges(obj, str) {

   var jQobj = $(obj);
   var row = jQobj.closest("tr");
   var id = row.attr("id");
   var srv = jQobj.closest("table").attr("id");
   var editVals = [];
   var bg = ((row.prevAll().length + 1) % 2 === 0) ? 'fefefe' : 'fff';

   $(':text', row).each(function () {
      editVals.push($(this).val());
   });

   var dto = { "id": id, "bg": bg, "vals": editVals };
   $.ajax({
      type: "post",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      url: "../../api/" + srv + ".asmx/Edit",
      data: JSON.stringify(dto),
      success: function (result) {
         var rt = result.d;
         if(rt.Success) {
            $(obj).parent().parent().parent().after(rt.Data).remove();
            ShowStatus("success", rt.Message);
         }
         else {
            ShowStatus("warning", rt.Message);
         }
      }
   });
}


function CancelChanges(obj, str) {
   var jObj = $(obj);
   var row = jObj.closest("tr");
   var id = row.attr("id");
   var bg = ((row.prevAll().length + 1) % 2 === 0) ? 'fefefe' : 'fff';

   jObj.parent().parent().parent().after('<tr id="' + id + '" bgcolor="#' + bg + '">' + str + '</tr>').remove();
   return false;
}

function DeleteRow(obj) {
   var id = $(obj).closest("tr").attr("id");
   var srv = $(obj).closest("table").attr("id");
   var that = $("[id$='" + id + "']");
   var dto = { "id": id };

   $.ajax({
      url: "../../api/" + srv + ".asmx/Delete",
      data: JSON.stringify(dto),
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
         var rt = result.d;
         if(rt.Success) {
            $(that).fadeOut(500, function () {
               $(that).remove();
            });
            ShowStatus("success", rt.Message);
         }
         else {
            ShowStatus("warning", rt.Message);
         }
      }
   });
   return false;
}

//--------------	LOAD DATA VIEWS

function LoadView() {
   if(location.href.indexOf("Users/Roles.aspx") != -1) {
      LoadRoles();
   }

   if(location.href.indexOf("Users/Users.aspx") != -1) {
      LoadUsers();
   }

   if(location.href.indexOf("Users/Profile.aspx") != -1) {
      LoadProfile();
   }
}

function LoadComments(pg, srvs) {
   $.ajax({
      url: srvs + "/LoadComments",
      data: "{'PageSize':'" + pageSize + "', 'Page':'" + pg + "'}",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
         $('#Container').setTemplateURL('../../Templates/comments.htm', null, { filter_data: false });
         $('#Container').processTemplate(msg);
         LoadPager(pg, srvs);
      }
   });
   return false;
}

function LoadPager(pg, srvs) {
   $.ajax({
      url: srvs + "/LoadPager",
      data: "{'PageSize':'" + pageSize + "', 'Page':'" + pg + "'}",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
         $('#Pager').html(msg.d);
      }
   });
   return false;
}

function LoadRoles() {
   $.ajax({
      url: "Roles.aspx/GetRoles",
      data: "{ }",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
         $('#Container').setTemplateURL('../../Templates/roles.htm', null, { filter_data: false });
         $('#Container').processTemplate(msg);
      }
   });
}

function LoadUsers() {
   $.ajax({
      url: "Users.aspx/GetUsers",
      data: "{ }",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
         $('#Container').setTemplateURL('../../Templates/users.htm', null, { filter_data: false });
         $('#Container').processTemplate(msg);
      }
   });
}

function LoadProfile() {
   var dto = { "id": Querystring('id') };
   $.ajax({
      url: "Profile.aspx/GetProfile",
      data: JSON.stringify(dto),
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
         $('#Container').setTemplateURL('../../Templates/profile.htm', null, { filter_data: false });
         $('#Container').processTemplate(msg);

         $('#Container2').setTemplateURL('../../Templates/profile2.htm', null, { filter_data: false });
         $('#Container2').processTemplate(msg);
      }
   });
}

//--------------    COMMENTS

var rowLoading = '<td colspan="8" style="text-align:center"><img src="../../pics/ajax-loader.gif" alt="Loading" /></td>';

function ProcessSelected(action, page) {
   var vals = [];

   // store the rows so they don't need to be queried for again.
   var commentsAndRows = [];
   // action: approve, reject, restore or delete
   var srv = "../../api/Comments.asmx/" + action;

   // Gets all checkboxes inside the #Comments table to prevent selecting
   // checkboxes that aren't part of the comments list.
   var checkedComments = $('#Comments input[@type=checkbox]:checked');

   if(checkedComments.length > 0) {

      checkedComments.each(function () {
         var jThis = $(this);

         // Check for the selectall checkbox otherwise this will throw an error.
         if(jThis.attr("id") != "selectall") {

            var row = jThis.closest("tr");
            var id = row.attr("id");
            commentsAndRows.push({
               row: row,
               id: id
            });

            vals.push(id);
         }
      });

      $('.loader').show();

      var dto = { "vals": vals };
      $.ajax({
         url: srv,
         data: JSON.stringify(dto),
         type: "post",
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         success: function (result) {

            var rt = result.d;
            if(rt.Success) {

               // Reference the counters so they don't need to be requeried
               // by each checkbox.
               var comment_counter = $('#comment_counter');
               var spam_counter = $('#spam_counter');
               var pingback_counter = $('#pingback_counter');
               var pending_counter = $('#pending_counter');


               // parse the current counts
               // Change these values before setting it to the element's text.
               var com_cnt = parseInt(comment_counter.text(),10);
               var spm_cnt = parseInt(spam_counter.text(), 10);
               var pbk_cnt = parseInt(pingback_counter.text(), 10);
               var pnd_cnt = parseInt(pending_counter.text(), 10);


               $.each(commentsAndRows, function (index, value) {

                  var row = value.row;
                  row.fadeOut(500, function () {
                     row.remove();
                  });

                  switch(action) {
                     case "Reject":
                        spm_cnt += 1;

                        switch(page) {
                           case "Approved": (com_cnt -= 1); break;
                           case "Pending": pending_counter.text((pnd_cnt - 1)); break;
                        }
                        break;

                     case "Approve":
                        com_cnt += 1;

                        switch(page) {
                           case "Pending": (pnd_cnt -= 1); break;
                           case "Spam": (spm_cnt -= 1); break;
                        }
                        break;

                     case "Delete":
                        switch(page) {
                           case "Approved": (com_cnt -= 1); break;
                           case "Spam": (spm_cnt -= 1); break;
                           case "Pingback": (pbk_cnt -= 1); break;
                           case "Pending": (pnd_cnt -= 1); break;
                        }
                        break;

                     default:
                        throw new Error("Unknown action: " + action);
                  }
               });

               spam_counter.text(spm_cnt);
               comment_counter.text(com_cnt);
               pending_counter.text(pnd_cnt);
               pingback_counter.text(pbk_cnt);

               ShowStatus("success", "Updated");
            }
            else {
               ShowStatus("warning", rt.Message);
            }

            $('.loader').hide();

         }
      });

   }

   return false;
}

var editingComment = '';

function EditComment(id) {
   var oRow = $("[id$='" + id + "']");
   var hRow = oRow.html();
   editingComment = hRow;

   var dto = { "id": id };
   $.ajax({
      url: "Approved.aspx/GetComment",
      data: JSON.stringify(dto),
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
         oRow.setTemplateURL('../../Templates/editcomment.htm', null, { filter_data: false });
         oRow.processTemplate(result);
      }
   });
   return false;
}

function SaveComment(obj) {
   var frm = document.forms.aspnetForm;
   $(frm).validate({
      rules: {
         txtAuthor: {
            required: true,
            maxlength: 30
         },
         txtComment: {
            required: true,
            maxlength: 2000
         },
         txtEmail: {
            required: true,
            email: true
         }
      }
   });

   var isValid = $(frm).valid();
   if(!isValid) { return false; }

   var oRow = $(obj).closest("tr");
   var vals = [];

   vals[0] = $(obj).closest("tr").attr("id");
   vals[1] = $("#txtAuthor").val();
   vals[2] = $("#txtEmail").val();
   vals[3] = $("#txtWebsite").val();
   vals[4] = $("#txtComment").val();

   var dto = { "vals": vals };
   $.ajax({
      url: "Approved.aspx/SaveComment",
      data: JSON.stringify(dto),
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
         oRow.setTemplateURL('../../Templates/commentrow.htm', null, { filter_data: false });
         oRow.processTemplate(result);
      }
   });

   return false;
}

function CancelEditComment(obj) {
    var oRow = $(obj).closest("tr");
    $(oRow).html(editingComment);
}

function DeleteComment(id) {
   var oRow = $("[id$='" + id + "']");
   var hRow = oRow.html();

   var loader = '<td colspan="8" style="text-align:center"><img src="../../pics/ajax-loader.gif" alt="Loading" /></td>';
   oRow.html(loader);

   var vals = [];
   vals[0] = id;
   var dto = { "vals": vals };
   $.ajax({
      url: "../../api/Comments.asmx/Delete",
      data: JSON.stringify(dto),
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
         var rt = result.d;
         if(rt.Success) {
            var com_cnt = $('#comment_counter').text();
            var spm_cnt = $('#spam_counter').text();
            var pbk_cnt = $('#pingback_counter').text();
            var pnd_cnt = $('#pending_counter').text();

            if(location.href.indexOf('Comments\/Approved.aspx') > 0) { $('#comment_counter').text(parseInt(com_cnt, 10) - 1); }
            if(location.href.indexOf('Comments\/Spam.aspx') > 0) { $('#spam_counter').text(parseInt(spm_cnt, 10) - 1); }
            if(location.href.indexOf('Comments\/Pingback.aspx') > 0) { $('#pingback_counter').text(parseInt(pbk_cnt, 10) - 1); }
            if(location.href.indexOf('Comments\/Pending.aspx') > 0) { $('#pending_counter').text(parseInt(pnd_cnt, 10) - 1); }

            $(oRow).fadeOut(500, function () {
               $(oRow).remove();
            });
            ShowStatus("success", rt.Message);
         }
         else {
            oRow.html(hRow);
            ShowStatus("warning", rt.Message);
         }
      }
   });
   return false;
}

function DeleteAllSpam() {
   $('.loader').show();
   $.ajax({
      url: "../../api/Comments.asmx/DeleteAll",
      data: "{ }",
      type: "POST",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
         var rt = result.d;
         if(rt.Success) {
            $('.chk').each(function () {
               var that = $("[id$='" + $(this).closest("tr").attr("id") + "']");
               $(that).fadeOut(500, function () {
                  $(that).remove();
               });
               ShowStatus("success", rt.Message);
               $('#spam_counter').text('0');
            });
         }
         else {
            ShowStatus("warning", rt.Message);
         }
      }
   });
   $('.loader').hide();
   return false;
}

//--------------HELPERS AND MISC

function toggleAllChecks(o) {
   if($(o).attr('checked')) {
      $('.chk').not(':disabled').attr('checked', 'checked');
   }
   else {
      $('.chk').attr('checked', '');
   }
   return false;
}

function formatJSONDate(jsonDate) {
   var d = new Date(parseInt(jsonDate.substr(6), 10));
   var nullDate = new Date(1001, 0, 1);

   if(d.getTime() <= nullDate.getTime()) {
      return "";
   }
   else {
      var m = d.getMonth() + 1;
      var s = m + "/" + d.getDate() + "/" + d.getFullYear();
      return s;
   }
}


function Querystring(key) {
   key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
   var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
   var qs = regex.exec(window.location.href);
   if(qs === null) {
      return "";
   }
   else {
      return qs[1];
   }
}

//--------------	STATUS AND MESSAGES

function ShowStatus(status, msg) {
   var adminStatus = $("[id$='AdminStatus']");
   adminStatus.removeClass("warning");
   adminStatus.removeClass("success");
   adminStatus.addClass(status);

   if(status == "success") {
      adminStatus.html(msg);
      adminStatus.fadeIn(1000, function () { }).delay(5000).fadeOut(1000, function () { });
   }
   else {
      adminStatus.html(msg + '<a href="javascript:HideStatus()" style="padding-left:20px; color:#444">close</a>');
      adminStatus.fadeIn(1000, function () { });
   }
}

function HideStatus() {
   $("[id$='AdminStatus']").slideUp('slow', function () { });
}

function Show(element) {
   $("[id$='" + element + "']").slideDown('slow', function () { });
   return false;
}

function Hide(element) {
   $("[id$='" + element + "']").slideUp('slow', function () { });
   return false;
}

function Toggle(element) {
    if ($("[id$='" + element + "']").is(':visible'))
        $("[id$='" + element + "']").slideUp('slow', function () { });
    else
        $("[id$='" + element + "']").slideDown('slow', function () { });
    return false;
}
