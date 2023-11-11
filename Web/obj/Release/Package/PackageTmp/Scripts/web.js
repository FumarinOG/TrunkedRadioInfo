var Common = (function () {

    var htmlDecode = function (value) {
        return $("<div />").html(value).text();
    },

        getSearchData = function (searchData) {

            if (searchData.length === 0) {
                return null;
            } else {
                return JSON.parse(htmlDecode(searchData));
            }
        },

        getSearchDataEncoded = function (searchData) {

            if (searchData === null) {
                return null;
            }

            return JSON.stringify(searchData);
        };

    return {
        HTMLDecode: htmlDecode,
        GetSearchData: getSearchData,
        GetSearchDataEncoded: getSearchDataEncoded
    };
})();