
var apiUrl = 'http://localhost:56490/api/';

function fileUpload() {
    $('#txtUploadFile').on('change',
        function (e) {
             
            CreateTempFileForViewing();

        });
}


function CreateTempFileForViewing() {

    var imgPlacekeeper = "'http://via.placeholder.com/300x300'";

    if (window.FormData !== undefined) {
        var data = new FormData();
         
        var file = document.getElementById("txtUploadFile").files[0];
        var myID = "Create";
        data.append(myID, file );


        $.ajax({
            type: "POST",
            url: '/api/imageupload/' + myID,
            contentType: false,
            processData: false,
            data: data,
           success: function (result) {
               var tmpVar = result.toString();
               var anchor = $('#picArea');
               anchor.empty();

               var anchor2 = $('#imgName');
               anchor2.val(tmpVar);

               var imgPath = '../Images/' + tmpVar;

               var row = '<img id="upimg" src="' + imgPath + '"';

               row += ' onerror ="this.src=' + imgPlacekeeper + '">'; 
                
                 
               
               anchor.append(row);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] === "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });
    } else {
        alert("This browser doesn't support HTML5 file uploads!");
    }

}

function fileSave(nFile) {
    var anchor = $('#picArea');
    var ofile = $('#imgName').val();
    var nfile = "TestImage.jpg";

    $.ajax({
        type: 'POST',
        url: '/api/saveimage/ ' ,
        data: JSON.stringify({
            OldFile: ofile,
            NewFile: nfile
           
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        'dataType': 'json',
        success: function() {
            anchor.empty();
        }

    });

   
}


function fileDelete(nFile) {
   
    var anchor = $('#picArea');
    var ofile = $('#imgName').val();

    $.ajax({
        type: 'POST',
        url: '/api/deleteimage/ ',
        data: JSON.stringify({
            OldFile: ofile,
            NewFile: nfile

        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        'dataType': 'json',
        success: function () {
            anchor.empty();
            
        }

    });

}