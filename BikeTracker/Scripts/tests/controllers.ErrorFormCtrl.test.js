describe('ErrorFormCtrl', function () {
    beforeEach(module("appControllers"));

    var deleteController, scope, modelInstance, testName;

    beforeEach(inject(function ($controller) {
        scope = {};

        modelInstance = {
            close: function () { },
        };
        spyOn(modelInstance, 'close');

        testTitle = "TestTitle1";
        testMessage = "TestMessage1";

        deleteController = $controller('ErrorFormCtrl', {
            $scope: scope,
            $uibModalInstance: modelInstance,
            title: testTitle,
            message: testMessage
        });
    }));

    it('should set the title and messages properties of scope to the provided values', function () {
        expect(scope.title).toBe(testTitle);
        expect(scope.message).toBe(testMessage);
    });

    it('should close the modal when the close binding is called', function () {
        scope.close();

        expect(modelInstance.close).toHaveBeenCalled();
    });
})