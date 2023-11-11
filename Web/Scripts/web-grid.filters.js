var Filters = (function () {

    var checkItem = function (dropDownList, title, removeItemNumber) {

        if (dropDownList.attr("title") === title) {
            var dropDown = dropDownList.data("kendoDropDownList");
            var itemToRemove = dropDown.dataSource.at(removeItemNumber);

            dropDown.dataSource.remove(itemToRemove);
            dropDown.select(0);
            return true;
        }

        return false;
    },

        filterNumericFormat = function (gridType, idField) {
            var numericMenu;

            if (typeof idField === "undefined") {
                numericMenu = $("#" + gridType + "sGrid thead th[data-field=" + gridType + "ID]").data("kendoFilterMenu");
            } else {
                numericMenu = $("#" + gridType + "sGrid thead th[data-field=" + idField + "]").data("kendoFilterMenu");
            }

            $(numericMenu.form).find("[data-role=numerictextbox]").each(function () {
                $(this).data("kendoNumericTextBox").setOptions({
                    format: "#"
                });
            });

            $(numericMenu.form).find("[data-role=dropdownlist]").each(function () {

                if (checkItem($(this), "Operator", 1)) {
                    return;
                }

                if (checkItem($(this), "Filters logic", 1)) {
                    return;
                }

                if (checkItem($(this), "Additional operator", 0)) {
                    return;
                }
            });
        },

        filterDateFormat = function (gridType, fieldName) {
            var dateMenu = $("#" + gridType + "sGrid thead th[data-field=" + fieldName + "]")
                .data("kendoFilterMenu");

            $(dateMenu.form).find("[data-role=datepicker]").each(function () {
                $(this).data("kendoDatePicker").setOptions({
                    format: "MM-dd-yyyy"
                });
            });

            $(dateMenu.form).find("[data-role=dropdownlist]").each(function () {

                if (checkItem($(this), "Operator", 1)) {
                    return;
                }

                if (checkItem($(this), "Filters logic", 1)) {
                    return;
                }

                if (checkItem($(this), "Additional operator", 0)) {
                    return;
                }
            });
        },

        systemFilterFormats = function () {
            filterNumericFormat("System");
            filterDateFormat("System", "FirstSeen");
            filterDateFormat("System", "LastSeen");
        },

        talkgroupFilterFormats = function () {
            filterNumericFormat("Talkgroup");
            filterDateFormat("Talkgroup", "FirstSeen");
            filterDateFormat("Talkgroup", "LastSeen");
        },

        radioFilterFormats = function () {
            filterNumericFormat("Radio");
            filterDateFormat("Radio", "FirstSeen");
            filterDateFormat("Radio", "LastSeen");
        },

        towerFilterFormats = function () {
            filterNumericFormat("Tower", "TowerNumber");
            filterDateFormat("Tower", "FirstSeen");
            filterDateFormat("Tower", "LastSeen");
        },

        patchFilterFormats = function () {
            filterNumericFormat("Patch", "FromTalkgroupID");
            filterNumericFormat("Patch", "ToTalkgroupID");
            filterDateFormat("Patch", "FirstSeen");
            filterDateFormat("Patch", "LastSeen");
        },

        processedFileFilterFormats = function () {
            filterDateFormat("Patch", "FirstSeen");
            filterDateFormat("Patch", "LastSeen");
        }

    return {
        SystemFilterFormats: systemFilterFormats,
        TalkgroupFilterFormats: talkgroupFilterFormats,
        RadioFilterFormats: radioFilterFormats,
        TowerFilterFormats: towerFilterFormats,
        PatchFilterFormats: patchFilterFormats,
        ProcessedFileFilterFormats: processedFileFilterFormats
    };
})();
