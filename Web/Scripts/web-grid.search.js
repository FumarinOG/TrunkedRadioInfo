var SystemSearches = (function () {

    var systemID = $("#SystemInfoSpan").data("system-id"),

        getTalkgroupSearchData = function () {

            var _activeOnly = false,
                _dateFrom = new Date(),
                _dateTo = new Date();

            if (typeof $("#TalkgroupViewDropDownList").data("kendoDropDownList") === "undefined") {
                _activeOnly = false;
            }
            else if ($("#TalkgroupViewDropDownList").data("kendoDropDownList").value() === "ActiveOnly") {
                _activeOnly = true;
            }

            if (typeof $("#TalkgroupDateFromPicker").data("kendoDatePicker") === "undefined") {
                _dateFrom = null;
            }
            else {
                _dateFrom = $("#TalkgroupDateFromPicker").data("kendoDatePicker").value();
            }

            if (typeof $("#TalkgroupDateToPicker").data("kendoDatePicker") === "undefined") {
                _dateTo = null;
            }
            else {
                _dateTo = $("#TalkgroupDateToPicker").data("kendoDatePicker").value();
            }

            return {
                systemID: systemID,
                activeOnly: _activeOnly,
                dateFrom: _dateFrom,
                dateTo: _dateTo
            };
        },

        getRadioSearchData = function () {

            return {
                systemID: systemID,
                activeOnly: ($("#RadioViewDropDownList").data("kendoDropDownList").value() === "ActiveOnly"),
                dateFrom: $("#RadioDateFromPicker").data("kendoDatePicker").value(),
                dateTo: $("#RadioDateToPicker").data("kendoDatePicker").value()
            };
        },

        getTowerSearchData = function () {

            return {
                systemID: systemID,
                activeOnly: ($("#TowerViewDropDownList").data("kendoDropDownList").value() === "ActiveOnly"),
                dateFrom: $("#TowerDateFromPicker").data("kendoDatePicker").value(),
                dateTo: $("#TowerDateToPicker").data("kendoDatePicker").value()
            };
        },

        getPatchSearchData = function () {

            return {
                systemID: systemID,
                dateFrom: $("#PatchDateFromPicker").data("kendoDatePicker").value(),
                dateTo: $("#PatchDateToPicker").data("kendoDatePicker").value()
            };
        },

        getProcessedFileSearchData = function () {

            return {
                systemID: systemID
            };
        };

    return {
        GetTalkgroupSearchData: getTalkgroupSearchData,
        GetRadioSearchData: getRadioSearchData,
        GetTowerSearchData: getTowerSearchData,
        GetPatchSearchData: getPatchSearchData,
        GetProcessedFileSearchData: getProcessedFileSearchData
    };
})(),

    DetailSearches = (function () {

        var getSearchData = function () {

            return {
                searchData: Common.GetSearchDataEncoded(_searchData)
            };
        };

        return {
            GetSearchData: getSearchData
        };
    })();