/*global ko*/

(function (ns) {
    "use strict";
    ns.LimitViewModel = function () {
        var self = this;
        self.limit = ko.observable(5);
        return {
            limit: self.limit
        };
    };
}(window.milosev));