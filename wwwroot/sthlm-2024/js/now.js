// Define the `nowApp` module
var nowApp = angular.module('nowApp', ['ng-trunk8']).config(function (trunk8ConfigProvider) {
    trunk8ConfigProvider.setConfig({
        lines: 2
    })
});

// Define the `NowController` controller on the `nowApp` module
nowApp.controller('NowController', function NowController($scope, $http, $interval) {
    $scope.slots = [];
    $scope.currentSlot = null;
    $scope.nextSlot = null;

    refreshFeed = function () {
        $http.get('now-feed').then(function (response) {
            $scope.slots = response.data;
            for (var n = 0; n < $scope.slots.length; n++) {
                $scope.slots[n].start = Date.parse($scope.slots[n].start);
                $scope.slots[n].end = Date.parse($scope.slots[n].end);
            }
            filterSlots();
        })
    };

    filterSlots = function () {
        // Ugly mod to fix date error onsite
        var now = new Date()-60000;

        var nextSlotIndex = 0;
        var currentSlotIndex = null;

        for (var n = 0; n < $scope.slots.length; n++) {
            if ($scope.slots[n].start <= now && $scope.slots[n].end >= now) {
                currentSlotIndex = n;
            } else if ($scope.slots[n].start > now) {
                break;
            }

            nextSlotIndex++;
        }

        if (currentSlotIndex != null) {
            $scope.currentSlot = $scope.slots[currentSlotIndex];
        } else {
            $scope.currentSlot = null;
        }

        if (nextSlotIndex < $scope.slots.length) {
            $scope.nextSlot = $scope.slots[nextSlotIndex];
        } else {
            $scope.nextSlot = null;
        }
    }

    refreshFeed();

    $interval(refreshFeed, 30 * 60000);
    $interval(filterSlots, 60000);

});
