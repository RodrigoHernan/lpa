// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function(){
    $('.dropdown-toggle').dropdown()
});

nunjucks.configure('/templates', { autoescape: true });
htmx.defineExtension('client-side-templates', {
    transformResponse : function(text, xhr, elt) {
        var nunjucksTemplate = htmx.closest(elt, "[nunjucks-template]");
        if (nunjucksTemplate) {
            var data = JSON.parse(text);
            var templateName = nunjucksTemplate.getAttribute('nunjucks-template');
            if (templateName.startsWith('#')) {
                var template = htmx.find('#' + templateName);
                return nunjucks.renderString(template.innerHTML, {data});
            } else {
                return nunjucks.render(templateName, {data});
            }
          }

        return text;
    }
});