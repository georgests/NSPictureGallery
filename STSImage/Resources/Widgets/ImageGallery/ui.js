(function (j) {
    if (typeof j.sts === 'undefined')
        j.sts = {};

    if (typeof j.sts.imggallery === 'undefined')
        j.sts.imggallery = {};

    if (typeof j.sts.imggallery.widgets === 'undefined')
        j.sts.imggallery.widgets = {};

    if (typeof j.sts.imggallery.widgets.imageWidget === 'undefined')
        j.sts.imggallery.widgets.imageWidget = {};

    var _updateDatabase = function (context, imgUrl) {
        var iName = context.imagename.val();
        var iUrl = imgUrl;

        if (iName === null || (iName !== null && iName.length === 0)) {
            alert("Please enter Image title and try again...");
            return;
        }

        if (iUrl === null || (iUrl !== null && iUrl.length == 0)) {
            alert("Click Browse first to to select your picture...");
            return;
        }
        j.telligent.evolution.post({
            url: j.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/imagegallerys/addimage.json',
            data:
			{
			    Name: context.imagename.val(),
			    filePath: imgUrl
			},
            success: function (response) {
                // alert("SUCCESS: " + response);
                // context.imagename.val('');
                _refreshAddImageView();
            },
            error: function (xhr, desc, ex) {
                context.imagename.val('')
                //                alert("ERROR: " + desc);
            }
        });
    };

    var _refreshAddImageView = function () {
        var context = j.sts.imggallery.widgets.imageWidget.context;
        j.telligent.evolution.get({
            url: context.stsAddImageURL,
            success: function (response) {
                j(context.stsAddFileContainer).html(response);
                _updateImageGalleryView(context);
            }
        });
    };

    var _updateImageGalleryView = function (context) {
        j.telligent.evolution.get({
            url: context.stsImageGalleryViewURL,
            success: function (response) {
                j(context.stsImageGallery).html(response);
            }
        });
    };

    var _deleteImage = function (imageId) {
        //        alert(imageId);
        var r = confirm("Do you want to delete this image?");
        if (r === false) {
            return;
        }

        j.telligent.evolution.post({
            url: j.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/imagegallerys/deleteimage.json',
            data:
			{
			    imageId: imageId
			},
            success: function (response) {
                //                alert("SUCCESS: " + response);
                _updateImageGalleryView(j.sts.imggallery.widgets.imageWidget.context);
            },
            error: function (xhr, desc, ex) {
                alert("Fail to Delete Image, Contact Administrator");
            }
        });
    };

    var _editImageTitle = function (imageId) {
        var tbImageTitle = jQuery('#tb' + imageId);
        var editBtn = jQuery('#btnedit' + imageId);
        var context = j.sts.imggallery.widgets.imageWidget.context;

        if (j.sts.imggallery.widgets.imageWidget.isEditActive == true && tbImageTitle.attr('readOnly') == 'readonly') {
            alert("Please complete earlier selected edit image activity");
            return;
        }

        if (tbImageTitle.attr('readOnly') == 'readonly') {
            tbImageTitle.attr('readOnly', false);
            tbImageTitle.removeClass('image_head_box_readonly deactivate');
            tbImageTitle.addClass('image_head_box activate');
            tbImageTitle.focus();
            editBtn.removeClass('edit_text_bttn');
            editBtn.addClass('edit_done_bttn');
            editBtn.attr('title', 'Accept Changes');
            j.sts.imggallery.widgets.imageWidget.isEditActive = true;
            return;
        } else {
            var title = tbImageTitle.attr('value');
            //            alert("in _editImageTitle -- Title: " + title);
            if (title === null || (title !== null && title.length === 0)) {
                alert("Please enter Image Title and try again");
                return;
            }

            tbImageTitle.attr('readOnly', true);
            editBtn.removeClass('edit_done_bttn');
            editBtn.addClass('edit_text_bttn');
            tbImageTitle.removeClass('image_head_box activate');
            tbImageTitle.addClass('image_head_box_readonly deactivate');
            //            editBtn.removeClass('edit_text_bttn');
            //            editBtn.addClass('edit_text_bttn');
            editBtn.attr('title', 'Edit Image Title');
            j.sts.imggallery.widgets.imageWidget.isEditActive = false;

            var r = confirm("Do you want to save changes?");
            if (r === false) {
                _updateImageGalleryView(context);
                return;
            }

            j.telligent.evolution.post({
                url: j.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/imagegallerys/updatetitle.json',
                data:
			    {
			        imageId: imageId,
			        title: title
			    },
                success: function (response) {
                    //                    alert("SUCCESS: " + response);
                    _updateImageGalleryView(j.sts.imggallery.widgets.imageWidget.context);
                },
                error: function (xhr, desc, ex) {
                    alert("Fail to Edit Image Title, Contact Administrator");
                }
            });
        }
    };


    var _rotateimage = function (imageId, degree) {

        if (degree == 360) {
            degree = 90;
        }
        else {
            degree = parseInt(degree) + parseInt(90);
        }
        var context = j.sts.imggallery.widgets.imageWidget.context;

        j.telligent.evolution.post({
            url: j.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/imagegallerys/updatedegree.json',
            data:
            {
                imageId: imageId,
                degree: degree
            },
            success: function (response) {

                _updateImageGalleryView(j.sts.imggallery.widgets.imageWidget.context);
            },
            error: function (xhr, desc, ex) {
                alert("Fail to Rotate Image, Contact Administrator");
            }
        });
    };


    var onLike = function (contentId, contentTypeId, typeId, complete) {
        //        alert("On Like");
        var data = {
            ContentId: contentId,
            ContentTypeId: contentTypeId
        };
        if (typeId !== null && typeId.length > 0) {
            data.TypeId = typeId;
        }
        $.telligent.evolution.post({
            url: j.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/likes.json',
            data: data,
            cache: false,
            dataType: 'json',
            success: function (response) {
                //                alert("On Like: " + response);
                // getCurrentCount(contentId, contentTypeId, typeId, complete);
            }
        });
    }

    var onUnlike = function (contentId, contentTypeId, typeId, complete) {
        //        alert("On unLike");
        var data = {
            ContentId: contentId
        };
        if (typeId !== null && typeId.length > 0) {
            data.TypeId = typeId;
        }
        j.telligent.evolution.del({
            url: $.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/like.json',
            data: data,
            cache: false,
            dataType: 'json',
            success: function (response) {
                //                alert("On unLike: " + response);
                // getCurrentCount(contentId, contentTypeId, typeId, complete);
            }
        });
    }


    var _showNext = function () {
        //        alert("List Size: " + j.sts.imggallery.widgets.imageWidget.imgData.length);

        if (j.sts.imggallery.widgets.imageWidget.selectionIndex >= (j.sts.imggallery.widgets.imageWidget.imgData.length - 1)) {
            return false;
        }

        var context = j.sts.imggallery.widgets.imageWidget.context;
        //    alert(j.sts.imggallery.widgets.imageWidget.selectionIndex);
        j.sts.imggallery.widgets.imageWidget.selectionIndex++;
        //        alert(j.sts.imggallery.widgets.imageWidget.selectionIndex);
        var obj = j.sts.imggallery.widgets.imageWidget.imgData[j.sts.imggallery.widgets.imageWidget.selectionIndex];
        var imageId = obj.imageId;
        var imageUrl = obj.imageUrl;
        var degree = obj.degree;
        j(context.stsSrcImage).attr("src", imageUrl);

        //  j(context.stsSrcImage).css({ transform: "rotate(" + degree + "deg)" });
        j(context.stsSrcImage).css({ '-webkit-transform': "rotate(" + degree + "deg)" });


        j.sts.imggallery.widgets.imageWidget.contentId = imageId;
        return false;

        /*
        context.stsPopup.style.position = "fixed";
        j(context.stsPopup).style.left = "50%";
        j(context.stsPopup).style.top = "50%";
        j(context.stsPopup).style.marginTop = "-350px";
        j(context.stsPopup).style.marginLeft = "-350px";
        */

        //        refreshComments(j.sts.imggallery.widgets.imageWidget.contentId, j.sts.imggallery.widgets.imageWidget.contentTypeId);
        //        var likeFormat = "{toggle} <span class='icon'></span> <span>{message}</span> <span class='count'>{count}</span>";
        // var likeFormat = '{toggle} {count}, {count} - {message}';
        //       j(context.stsImageLike).evolutionLike({ contentId: j.sts.imggallery.widgets.imageWidget.contentId, contentTypeId: j.sts.imggallery.widgets.imageWidget.contentTypeId, format: likeFormat, onLike: onLike, onUnlike: onUnlike });
    };

    var _showPrev = function () {
        if (j.sts.imggallery.widgets.imageWidget.selectionIndex === 0) {
            return false;
        }

        var context = j.sts.imggallery.widgets.imageWidget.context;
        //        alert(j.sts.imggallery.widgets.imageWidget.selectionIndex);
        j.sts.imggallery.widgets.imageWidget.selectionIndex--;
        //      alert(j.sts.imggallery.widgets.imageWidget.selectionIndex);
        var obj = j.sts.imggallery.widgets.imageWidget.imgData[j.sts.imggallery.widgets.imageWidget.selectionIndex];
        var imageId = obj.imageId;
        var imageUrl = obj.imageUrl;
        var degree = obj.degree;
        j(context.stsSrcImage).attr("src", imageUrl);

        // j(context.stsSrcImage).css({ transform: "rotate(" + degree + "deg)" });
        j(context.stsSrcImage).css({ '-webkit-transform': "rotate(" + degree + "deg)" });

        j.sts.imggallery.widgets.imageWidget.contentId = imageId;
        return false;

        //       refreshComments(j.sts.imggallery.widgets.imageWidget.contentId, j.sts.imggallery.widgets.imageWidget.contentTypeId);
        //       var likeFormat = "{toggle} <span class='icon'></span> <span>{message}</span> <span class='count'>{count}</span>";
        // var likeFormat = '{toggle} {count}, {count} - {message}';
        //      j(context.stsImageLike).evolutionLike({ contentId: j.sts.imggallery.widgets.imageWidget.contentId, contentTypeId: j.sts.imggallery.widgets.imageWidget.contentTypeId, format: likeFormat, onLike: onLike, onUnlike: onUnlike });
    };

    var _updateImageArray = function (img, contentId, contentTypeId, imageURL, imageId) {
        var context = j.sts.imggallery.widgets.imageWidget.context;
        j.sts.imggallery.widgets.imageWidget.imgData = [];
        j.sts.imggallery.widgets.imageWidget.selectionIndex = 0;

        //        alert("Children Count: " + context.stsImageGallery.children().length);
        context.stsImageGallery.children().each(function (index) {
            var tImageId = j(this).attr("data-image-id");
            var tImageUrl = j(this).attr("data-image-url");
            var tImgDegree = j(this).attr("img-degree")

            //            alert("Image URL: " + tImageUrl + "  == " + tImageId);
            if (typeof tImageId == 'undefined' && typeof tImageUrl == 'undefined') {
                //                alert("UNDEFINED===================>> ");
            } else {
                var obj = new Object();
                obj.imageId = tImageId;
                obj.imageUrl = tImageUrl;
                obj.degree = tImgDegree;

                j.sts.imggallery.widgets.imageWidget.imgData.push(obj);
                if (imageId === tImageId) {
                    j.sts.imggallery.widgets.imageWidget.selectionIndex = index - 1;
                    //                    alert(j.sts.imggallery.widgets.imageWidget.selectionIndex);
                }
            }
        });
    };


    var _showPopup = function (img, contentId, contentTypeId, imageURL, imageId, degree) {
        j.sts.imggallery.widgets.imageWidget.isPopupOpen = true;
        //        alert("In _showPopup method - ContentId: " + contentId);
        j.sts.imggallery.widgets.imageWidget.contentId = contentId;
        j.sts.imggallery.widgets.imageWidget.contentTypeId = contentTypeId;
        var context = j.sts.imggallery.widgets.imageWidget.context;
        //        j(context.stsPopup).css({ display: "block" });
        j(context.stsSrcImage).attr("src", imageURL);
        if (degree == 0) {
            //  j(context.stsSrcImage).css({ transform: "rotate(0deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(0deg)" });
        }
        else if (degree == 90) {
            // j(context.stsSrcImage).css({ transform: "rotate(90deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(90deg)" });
        }
        else if (degree == 180) {
            // j(context.stsSrcImage).css({ transform: "rotate(180deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(180deg)" });
        }

        else if (degree == 270) {
            // j(context.stsSrcImage).css({ transform: "rotate(270deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(270deg)" });
        }

        else if (degree == 360) {
            // j(context.stsSrcImage).css({ transform: "rotate(360deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(360deg)" });
        }
        else {
            // j(context.stsSrcImage).css({ transform: "rotate(0deg)" });
            j(context.stsSrcImage).css({ '-webkit-transform': "rotate(0deg)" });
        }


        //        var likeFormat = "{toggle} <span class='icon'></span> <span>{message}</span> <span class='count'>{count}</span>";
        // var likeFormat = '{toggle} {count}, {count} - {message}';
        //        j(context.stsImageLike).evolutionLike({ contentId: j.sts.imggallery.widgets.imageWidget.contentId, contentTypeId: j.sts.imggallery.widgets.imageWidget.contentTypeId, format: likeFormat, onLike: onLike, onUnlike: onUnlike });
        // j(context.stsImageLike).evolutionLike({ contentId: contentId, contentTypeId: contentTypeId });

        _updateImageArray(img, contentId, contentTypeId, imageURL, imageId);

        //        refreshComments(contentId, contentTypeId);

        //        j.sts.imggallery.widgets.imageWidget.popup.glowPopUpPanel('show');
        //        alert((j(context.stsPopup)).innerHTML);
        //                j(context.stsPopup).glowPopUpPanel('show');
        //         context.popup.glowPopUpPanel('html', 'Hellow World');
        //                 context.popup.glowPopUpPanel('show', j(context.stsImageGallery));
        //        j(context.stsPopup).dialog({ modal: true, height: 590, width: 1005 });
        var myPopup = Popup.showModal(context.imagePopup);
        //        myPopup.autoHide = false;
    };

    var _closePopup = function () {
        //        alert("In _closePopup method");
        var context = j.sts.imggallery.widgets.imageWidget.context;
        j(context.stsPopup).css({ display: "none" });
        //        j.sts.imggallery.widgets.imageWidget.popup.glowPopUpPanel('hide');
        // j(context.stsPopup).glowPopUpPanel('hide');
        j.sts.imggallery.widgets.imageWidget.isPopupOpen = false;
        Popup.hide(context.imagePopup);
    };

    var refreshComments = function (contentId, contentTypeId) {
        var context = j.sts.imggallery.widgets.imageWidget.context;
        j.telligent.evolution.get({
            url: context.stsCommentsListUrl,
            data:
			{
			    contentid: contentId,
			    contentyypeid: contentTypeId
			},
            success: function (response) {
                j(context.stsCommentsView).html(response);
            }
        });
    };

    var _addcomment = function () {
        var contentId = j.sts.imggallery.widgets.imageWidget.contentId;
        var contentTypeId = j.sts.imggallery.widgets.imageWidget.contentTypeId;
        var context = j.sts.imggallery.widgets.imageWidget.context;
        var bodyStr = j(context.stsAddCommentTextArea).val()

        if (bodyStr === null || (bodyStr !== null && bodyStr.length === 0)) {
            alert("Please enter Your Comment and try again");
            return;
        }
        jQuery.telligent.evolution.post({
            url: jQuery.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/comments.json',
            data: {
                ContentId: contentId,
                ContentTypeId: contentTypeId,
                Body: bodyStr
            },
            success: function (response) {
                refreshComments(contentId, contentTypeId);
            }
        });
    };

    j.sts.imggallery.widgets.imageWidget =
	{
	    register: function (context) {
	        j.sts.imggallery.widgets.imageWidget.context = context;
	        j.sts.imggallery.widgets.imageWidget.isEditActive = false;
	        j.sts.imggallery.widgets.imageWidget.isPopupOpen = false;


	        // _updateImageGalleryView(context);

	        j(document).keyup(function (e) {
	            if (j.sts.imggallery.widgets.imageWidget.isPopupOpen === false) {
	                return;
	            }

	            //	            alert("Key Code: " + e.keyCode);
	            if (e.keyCode == 27) {
	                //	                alert("Esc Key Pressed");
	                var context = j.sts.imggallery.widgets.imageWidget.context;
	                j(context.stsPopup).css({ display: "none" });
	                //        j.sts.imggallery.widgets.imageWidget.popup.glowPopUpPanel('hide');
	                // j(context.stsPopup).glowPopUpPanel('hide');
	                Popup.hide(context.imagePopup);
	                j.sts.imggallery.widgets.imageWidget.isPopupOpen = false;
	            }   // esc
	        });

	        /*
	        context.popup = $('<div></div>').glowPopUpPanel({
	        cssClass: 'menu group-navigation-content group-navigation-content__menu',
	        position: 'down',
	        zIndex: 1000,
	        hideOnDocumentClick: false
	        }).glowPopUpPanel('html', '');
	        */

	        _refreshAddImageView();
	    },
	    registerAddImage: function (con) {
	        var context = j.sts.imggallery.widgets.imageWidget.context;
	        context.imagename = con.imagename;
	        context.stsUploadFileCtrl = con.stsUploadFileCtrl;
	        // alert(con.stsFileUploadContextURL);
	        // context.stsFileUploadContextURL = con.stsFileUploadContextURL;
	        // context.stsFileUploadContextId = con.stsFileUploadContextId;
	        // context.stsUploadImageDataURL = con.stsUploadImageDataURL;
	        context.uploadFileButton = con.uploadFileButton;
	        context.fileUploadContainerId = con.fileUploadContainerId;

	        var file = j(context.fileUploadContainerId).glowUpload({
	            title: "Image files", extensions: "jpg,jpeg,gif,png",
	            uploadUrl: context.stsFileUploadContextURL
	        });

	        // j(context.stsAddCommentTextArea).evolutionComposer();

	        // j(context.stsImageGallery).lightBox();
	        /*
	        j.sts.imggallery.widgets.imageWidget.popup = j(context.stsPopup).glowPopUpPanel({
	        position: 'down',
	        zIndex: 1000,
	        hideOnDocumentClick: false
	        });
	        */

	        j(context.uploadFileButton).bind('click', function (e) {
	            // alert(context.imagename.val());
	            var f = j(context.fileUploadContainerId).glowUpload('val');
	            if (f.name === null || (f.name !== null && f.name.length === 0)) {
	                alert("Click Browse first to to select your picture");
	                return;
	            }
	            // alert(f.name);
	            // alert(context.stsUploadImageDataURL);
	            jQuery.telligent.evolution.get({
	                url: context.stsUploadImageDataURL,
	                data: {
	                    fileUploadContextId: context.stsFileUploadContextId,
	                    fileName: f.name
	                },
	                success: function (response) {
	                    // console.log(response);
	                    // alert(response.toString().trim());
	                    _updateDatabase(context, response.toString().trim());
	                }
	            });
	        });
	    },
	    deleteimage: function (imageId) {
	        _deleteImage(imageId);
	    },
	    editimagetitle: function (imageId) {
	        _editImageTitle(imageId);
	    },
	    rotateimage: function (imageId, degree) {
	        _rotateimage(imageId, degree);
	    },
	    showpopup: function (img, contentId, contentTypeId, imageUrl, imageId, degree) {
	        _showPopup(img, contentId, contentTypeId, imageUrl, imageId, degree);
	        return false;
	    },
	    closepopup: function () {
	        _closePopup();
	    },
	    addcomment: function () {
	        _addcomment();
	    },
	    showNext: function () {
	        _showNext();
	        return false;
	    },
	    showPrev: function () {
	        _showPrev();
	        return false;
	    }
	};
})(jQuery, window);
