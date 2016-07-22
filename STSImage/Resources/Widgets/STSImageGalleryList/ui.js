jQuery(function($){
    if (typeof $.sts === 'undefined')
        $.sts = {};

    if (typeof $.sts.imggallery === 'undefined')
        $.sts.imggallery = {};

    if (typeof $.sts.imggallery.widgets === 'undefined')
        $.sts.imggallery.widgets = {};

    if (typeof $.sts.imggallery.widgets.listImageGallary === 'undefined')
        $.sts.imggallery.widgets.listImageGallary = {};

    var _updateImageGalleryListView = function (context) {
        $.telligent.evolution.get({ url: context.stsImageGalleryListURL,
            success: function (response) {
                $(context.stsImageGalleryListView).html(response);
            }
        });
    };

    var _attachHandlers = function (context) {
        $(context.btnNewImageGallery).bind('click', function (e) {
//            var igName = $.trim(context.ImageTitle.val());
            var igName = $(context.ImageTitle).val();
            alert('Image Gallery Name: ' + context.ImageTitle.val() + " ==> " + igName);
            $.telligent.evolution.post({
                url: $.telligent.evolution.site.getBaseUrl() + 'api.ashx/v2/imagegallerys/imagegallery.json',
                data:
                {
                    Name: igName,
                },
                success: function(response)
                {
                    alert('New Image Gallery Created!');
                }
            });

            return false;
        });
    };

    $.sts.imggallery.widgets.listImageGallary = {
        register: function(context) {
            _attachHandlers(context);
            _updateImageGalleryListView(context);
        }
    };
});
