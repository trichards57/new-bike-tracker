interface IUser {
    EmailAddress: string;
    Role: string;
    Id: string;
}

class AdminController extends BaseController {
    constructor($scope: angular.IScope, $window: angular.IWindowService,
        $uibModal: angular.ui.bootstrap.IModalService, User: angular.resource.IResource<{}>) {
        super($scope, $uibModal, $window, "Reports Control Panel - SJA Tracker");

        this.$uibModal = $uibModal;
        this.User = User;
    }

    static $inject = ["$scope", "$window", "$uibModal", "User"];

    sortBy = "UserName";
    sortReverse = false;
    userFilter = "";
    $uibModal: angular.ui.bootstrap.IModalService;
    users: IUser[];

    User: angular.resource.IResource<{}>;

    showAscending(param: string) {
        return this.sortBy !== param || (this.sortBy == param && !this.sortReverse);
    }

    showUpdateUser(id: string) {
        let users = $.grep(this.users, function (e) {
            return e.Id === id;
        });

        let user: any;

        if (user.length === 1) {
            user = users[0];
        }
        else {
            this.showError("Couldn't Update User", "There was an error updating that user.  Please try again later.");
            return;
        }

        let modalInstance = this.$uibModal.open({
            animation: true,
            templateUrl: "/Admin/EditForm",
            controller: "EditUserCtrl as vm",
            resolve: {
                createMode: function () {
                    return false;
                },
                email: function () {
                    return user.EmailAddress;
                },
                role: function () {
                    return user.Role;
                }
            }
        });

        let vm = this;

        //modalInstance.result.then(function (res) {
        //    var i = vm.User.
        //});
    }
}

//angular.module("app").controller("AdminCtrl", AdminController);
