var siteUrl = 'http://localhost:56490/';

$(document).ready(function () {
    getComments();

});

function contentToHTML() {
    var html = $('#content-string').text();
    html = html.replace("<textarea>", "");
    //$('#content-string').empty();
    $('#blog-content').append(html);
}

function getBlogPost() {
    var siteUrl = "http://localhost:56490/api/blogpost/";

    $.ajax({
        type: 'GET',
        url: siteUrl,
        dataType: "text",
        data: {
            id: $('#Id').val()
        },
        contentType: 'application/json',
        
        success: function (html) {
            $('#blog-post').empty();
            $('#blog-post').append(html);
        },
        fail: function () {
            errorMessages();
        }
    });
}

function getComments() {
    var siteId = $('#Id').val();
  

    clearErrors();
    $.ajax({
        type: 'GET',
        url: "http://localhost:56490/api/" + siteId + "/comments/",
        contentType: 'application/json'
        
    })
        .done (function (results) {
            $('#comments').empty();
            $('#comments').append('<div style="font-family: Eater, cursive; color: red;"><h2>Comments</h2></div>');

            $.each(results, function (index, comment) {
                showComment(comment);
            });
        })
        .fail(function () {
            errorMessages();
        });
    //});
}

function postComment(userid) {

    var siteId = $('#Id').val();
    var newComment = $('#NewComment').val();

    clearErrors();
    $.ajax({
        type: 'POST',
        url: "http://localhost:56490/api/" + siteId + "/comments/",
        contentType: 'application/json',
        data: JSON.stringify ({
            PostId: siteId,
            Comment: newComment,
            UserId: userid
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        'dataType': 'json',
        success: function (results) {
            ////$('#comments').empty();
            //$('#comments').append('<div style="font-family: Eater, cursive; color: red;"><h2>Comments</h2></div>');

            $.each(results, function (index, comment) {
                showComment(comment);
            });
        },
        fail: function () {
            errorMessages();
        }
    });
}

function errorMessages() {
    $('#errorMessages')
        .append($('<li>')
            .attr({ "class": 'list-group-item list-group-item-danger' })
            .text('Error calling web service. Please try again later.'));
}

function clearErrors() {
    $('#errorMessages').empty();
}

function showComment(comment) {
    clearErrors();
    $.ajax({
        url: siteUrl + 'home/comment/',
        type: 'Get',
        contentType: 'application/json',
        data: {
            Id: comment.Id,
            PostId: comment.PostId,
            Comment: comment.Comment,
            CommentDate: comment.CommentDate,
            UserName: comment.UserName,
            DisplayName: comment.DisplayName
        }
    })
        .done(function (html) {
            $('#comments').append(html);
        })
        .fail(function () {
            errorMessages();
        });
        
    
}

function deleteComment(commentId) {

    var siteId = $('#Id').val();
    var newComment = $('#NewComment').val();

    clearErrors();
    $.ajax({
        type: 'POST',
        url: "http://localhost:56490/api/deletecomment/" + siteId,
        contentType: 'application/json',
        data: JSON.stringify({
            CommentId: commentId
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        'dataType': 'json',
        success: function () {
            
            $('#comment-' + commentId).hide();
        },
        fail: function () {
            errorMessages();
        }
    });
}