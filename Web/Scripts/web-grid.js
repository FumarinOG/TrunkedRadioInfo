var GridsCommon = (function () {

    var postSearch = function (urlData, searchData) {

        $("body").append($("<form/>")
            .attr({ "action": urlData, "method": "post", "id": "RedirectForm" })
            .append($('<input/>')
                .attr({ "type": "hidden", "name": "searchData", "value": JSON.stringify(searchData) })
            )
        ).find("#RedirectForm").submit();
    };

    return {
        PostSearch: postSearch
    };
})(),

    Grids = (function () {

        var setDefaultDates = function (dateFromPicker, dateToPicker) {

            if (dateFromPicker.value() === null) {
                dateToPicker.value(null);
            }

            if (dateToPicker.value() === null) {

                switch (Date.compare(dateFromPicker.value(), new Date())) {

                    case 0:
                        dateToPicker.value(new Date());
                        break;

                    case 1:
                        dateFromPicker.value(new Date());
                        dateToPicker.value(new Date());
                        break;

                    case -1:
                        dateToPicker.value(new Date());
                        break;
                }
            }
        },

            setDefaultIDs = function (idFromTextBox, idToTextBox) {

                if (idToTextBox.val().trim().length === 0) {

                    if (idFromTextBox.val().trim().length !== 0) {
                        idToTextBox.val(parseInt(idFromTextBox.val()) + 100);
                    }
                }

            },

            talkgroupsDataBound = function (arg) {
            },

            talkgroupViewDropDownListChange = function (arg) {
                $("#TalkgroupsGrid").data("kendoGrid").dataSource.read();
            },

            talkgroupChange = function (arg) {
                var selected = this.dataItem(this.select());

                GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewTalkgroupURL, _systemID, selected.TalkgroupID), SystemSearches.GetTalkgroupSearchData());
            },

            talkgroupIDFromChange = function (args) {
                setDefaultIDs($("#TalkgroupIDFromTextBox"), $("#TalkgroupIDToTextBox"));
            },

            talkgroupDateFromChange = function (args) {
                setDefaultDates($("#TalkgroupDateFromPicker").data("kendoDatePicker"), $("#TalkgroupDateToPicker").data("kendoDatePicker"));
            },

            radiosDataBound = function (arg) {
            },

            radioViewDropDownListChange = function (arg) {
                var grid = $("#RadiosGrid").data("kendoGrid");

                grid.dataSource.read();
            },

            radioChange = function (arg) {
                var selected = this.dataItem(this.select());

                GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewRadioURL, _systemID, selected.RadioID),
                    SystemSearches.GetRadioSearchData());
            },

            radioIDFromChange = function (args) {
                setDefaultIDs($("#RadioIDFromTextBox"), $("#RadioIDToTextBox"));
            },

            radioDateFromChange = function (args) {
                setDefaultDates($("#RadioDateFromPicker").data("kendoDatePicker"), $("#RadioDateToPicker").data("kendoDatePicker"));
            },

            towersDataBound = function (arg) {
            },

            towerChange = function (arg) {
                var selected = this.dataItem(this.select());

                GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewTowerURL, _systemID, selected.TowerNumber), SystemSearches.GetTowerSearchData());
            },

            towerDateFromChange = function (args) {
                setDefaultDates($("#TowerDateFromPicker").data("kendoDatePicker"), $("#TowerDateToPicker").data("kendoDatePicker"));
            },

            patchChange = function (arg) {
                var selected = this.dataItem(this.select());

                GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/{3}/",
                    _viewPatchURL,
                    _systemID,
                    selected.FromTalkgroupID,
                    selected.ToTalkgroupID),
                    SystemSearches.GetPatchSearchData());
            },

            patchDateFromChange = function (args) {
                setDefaultDates($("#PatchDateFromPicker").data("kendoDatePicker"), $("#PatchDateToPicker").data("kendoDatePicker"));
            },

            historyDataBound = function (arg) {
            },

            frequenciesDataBound = function (args) {
            };

        return {
            TalkgroupsDataBound: talkgroupsDataBound,
            TalkgroupViewDropDownListChange: talkgroupViewDropDownListChange,
            TalkgroupChange: talkgroupChange,
            TalkgroupIDFromChange: talkgroupIDFromChange,
            TalkgroupDateFromChange: talkgroupDateFromChange,
            RadiosDataBound: radiosDataBound,
            RadioViewDropDownListChange: radioViewDropDownListChange,
            RadioChange: radioChange,
            RadioIDFromChange: radioIDFromChange,
            RadioDateFromChange: radioDateFromChange,
            TowersDataBound: towersDataBound,
            TowerChange: towerChange,
            TowerDateFromChange: towerDateFromChange,
            PatchChange: patchChange,
            PatchDateFromChange: patchDateFromChange,
            HistoryDataBound: historyDataBound,
            FrequenciesDataBound: frequenciesDataBound
        };
    })(),

    TalkgroupGrids = (function () {

        var radioChange = function (arg) {
            var selected = this.dataItem(this.select());

            GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewRadioURL, _systemID, selected.RadioID), _searchData);
        };

        return {
            RadioChange: radioChange
        };
    })(),

    RadioGrids = (function () {

        var talkgroupChange = function (arg) {
            var selected = this.dataItem(this.select());

            GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewTalkgroupURL, _systemID, selected.TalkgroupID), _searchData);
        };

        return {
            TalkgroupChange: talkgroupChange
        };
    })(),

    TowerGrids = (function () {

        var talkgroupChange = function (arg) {
            var selected = this.dataItem(this.select());

            GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}/", _viewTalkgroupURL, _systemID, selected.TalkgroupID), _searchData);
        },

            radioChange = function (arg) {
                var selected = this.dataItem(this.select());

                GridsCommon.PostSearch(kendo.format("{0}/{1}/{2}", _viewRadioURL, _systemID, selected.RadioID), _searchData);
            };

        return {
            TalkgroupChange: talkgroupChange,
            RadioChange: radioChange
        };
    }),

    FrequenciesGrid = (function () {

        var frequencyChange = function (arg) {
            var selected = this.dataItem(this.select());

            GridsCommon.PostSearch(kendo.format("{0}/?frequency={1}", _viewFrequencyURL, encodeURIComponent(selected.Frequency)), _searchData)
        };

        return {
            FrequencyChange: frequencyChange
        };
    })();

