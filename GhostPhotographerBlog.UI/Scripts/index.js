var siteUrl = "http://localhost:56490/";

$(document).ready(function () {
    loadfirstblogs();

});
function loadfirstblogs() {
    
    //$.ajax({
    //    type: 'GET',
    //    url: siteUrl + 'Post/PostSnippet/',
    //    contentType: "application/JSON",
    //    data: {
    //        Id: BlogPostInfo.Id,
    //        Title: BlogPostInfo.Title,
    //        PostContent: BlogPostInfo.PostContent,
    //        PostImage: BlogPostInfo.PostImage,
    //        DisplayAuthor: BlogPostInfo.DisplayAuthor,
    //        DateCreated: BlogPostInfo.DateCreated,
    //        DateCreatedString: BlogPostInfo.DateCreatedString


    //    }
    //})
    //    .done(function (html) {
    //        $('#search-results').append(html);
    //    })
    //    .fail(function () {
    //        errorMessages();
    //    });
}

