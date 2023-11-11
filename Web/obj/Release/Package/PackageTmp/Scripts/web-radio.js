var RadioTalkgroupTowerGrids = (function () {

    var towerNumber,
        date,

        towerDataBound = function () {
            $("#TowerTalkgroupsGrid").data("kendoGrid").dataSource.read();
            $("#DateDropDownList").data("kendoDropDownList").dataSource.read();
        },

        towerChange = function () {
            $("#TowerTalkgroupsGrid").data("kendoGrid").dataSource.read();
        },

        getTowerNumber = function () {
            var selectedTower = $("#TowerDropDownList").data("kendoDropDownList").value();

            towerNumber = selectedTower;

            return {
                towerNumber: selectedTower,
                searchData: Common.GetSearchDataEncoded(_searchData)
            };
        },

        dateDataBound = function () {
        },

        dateChange = function () {
            $("#TowerTalkgroupsGrid").data("kendoGrid").dataSource.read();
        },

        getSelectedOptions = function () {
            var selectedTower = $("#TowerDropDownList").data("kendoDropDownList").value();
            var selectedDate = kendo.parseDate($("#DateDropDownList").data("kendoDropDownList").value());
            // Date properties stored in a Kendo DropDownList aren't really a "Date object" parseable by normal
            // means, however the Kendo parseDate() method understands them and return null for non-dates which
            // works out for this purpose

            date = selectedDate;

            return {
                towerNumber: selectedTower,
                date: selectedDate,
                searchData: Common.GetSearchDataEncoded(_searchData)
            };
        },

        setTowerTalkgroupRadiosFilename = function (args) {


            if (date === null) {
                args.workbook.fileName = kendo.format("TowerTalkgroups-Sys{0}-T{1}-R{2}.xlsx", _systemID, @Model.RadioID, towerNumber);
            }
            else {
                args.workbook.fileName = kendo.format("TowerRadios-Sys{0}-T{1}-TG{2}-{3:MM-dd-yyyy}.xlsx", _systemID, @Model.RadioID, towerNumber, date);
            }
        };

    return {
        TowerDataBound: towerDataBound,
        TowerChange: towerChange,
        GetTowerNumber: getTowerNumber,
        DateDataBound: dateDataBound,
        DateChange: dateChange,
        GetSelectedOptions: getSelectedOptions,
        SetTowerTalkgroupRadiosFilename: setTowerTalkgroupRadiosFilename
    };
})();
