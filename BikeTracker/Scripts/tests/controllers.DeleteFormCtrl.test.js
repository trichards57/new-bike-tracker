describe('DeleteFormCtrl', function () {
    beforeEach(module("appControllers"));

    var deleteController, scope, modelInstance, testName;

    beforeEach(inject(function ($controller) {
        scope = {};

        modelInstance = {
            close: function () { },
            dismiss: function (arg) { }
        };
        spyOn(modelInstance, 'close');
        spyOn(modelInstance, 'dismiss');

        testName = "TestName1";
        deleteController = $controller('DeleteFormCtrl', {
            $scope: scope,
            $modalInstance: modelInstance,
            name: testName
        });
    }));

    it('should set the name property of scope to the provided name', function () {
        expect(scope.name).toBe(testName);
    });

    it('should close the modal when the ok binding is called', function () {
        scope.ok();

        expect(modelInstance.close).toHaveBeenCalled();
    });

    it('should close the modal with a cancel when the cancel binding is called', function () {
        scope.cancel();
        expect(modelInstance.dismiss).toHaveBeenCalledWith("cancel");
    });
})