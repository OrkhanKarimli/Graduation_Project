$(document).ready(function () {
  //TODO: Reply

  let replyBtns = $(".content-tools-reply");

  $(replyBtns).on("click", function () {
    let replySide = $(this)
      .closest(".comments-content-tools")
      .next(".reply-side");

    if ($(replySide).css("display") == "none") {
      $(replySide).css("display", "block");
    } else {
      $(replySide).css("display", "none");
    }
  });

  //TODO: Comments

  let commentsContent = $(".comments-content");

  let filteredComments = $(commentsContent).filter((index, item) => {
    return $(item).text().length >= 450;
  });

  $.each(filteredComments, function (index, item) {
    let constComment = $("<p/>");
    $(constComment).addClass("const-comment");
    $(constComment).css("display", "none");
    $(constComment).text($(item).text());
    $(item).text(`${$(item).text().substr(0, 450)}...`);
    let commentContentTools = $(item).next(".comments-content-tools");
    let expandBtnCreate = `<a class="content-tools-expand"
       ><i class="fas expand-icon fa-expand-alt"></i>
       Davamını gör</a
       >`;
    $(item).before(constComment);
    $(commentContentTools).prepend(expandBtnCreate);
  });

  let expandBtn = $(".content-tools-expand");

  // console.log(contentsArr);

  $(expandBtn).on("click", function () {
    let dynamicCommentContent = $(this)
      .closest(".comments-content-tools")
      .prev(".comments-content");

    let constComment = $(dynamicCommentContent).prev(".const-comment");

    let thisI = $(this).find("i");

    if ($(thisI).hasClass("fa-expand-alt")) {
      $(dynamicCommentContent).css("display", "none");
      $(constComment).css("display", "block");

      $(this).html("");
      $(this).append(`<a class="content-tools-expand"
         ><i class="fas expand-icon fa-compress-alt"></i>
         Kiçilt</a
         >`);
    } else {
      $(dynamicCommentContent).css("display", "block");
      $(constComment).css("display", "none");

      $(this).html("");
      $(this).append(`<a class="content-tools-expand"
         ><i class="fas expand-icon fa-expand-alt"></i>
         Davamını gör</a
         >`);
    }
  });
});
