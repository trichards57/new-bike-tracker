describe('DeleteFormCtrl', function () {
    beforeEach(angular.mock.module("app"));
    beforeEach(angular.mock.module("appControllers"));

    let deleteController: DeleteFormController;
    let modelInstance: angular.ui.bootstrap.IModalServiceInstance;
    const testName = "TestName1";

    beforeEach(inject(function ($controller: angular.IControllerService) {
        modelInstance = jasmine.createSpyObj('$uibModalInstance', ['close', 'dismiss']);

        deleteController = $controller<DeleteFormController>('DeleteFormCtrl', {
            $uibModalInstance: modelInstance,
            name: testName
        });
    }));

    it('should set the name property of scope to the provided name', function () {
        expect(deleteController.name).toBe(testName);
    });

    it('should close the modal when the ok binding is called', function () {
        deleteController.ok();

        expect(modelInstance.close).toHaveBeenCalled();
    });

    it('should close the modal with a cancel when the cancel binding is called', function () {
        deleteController.cancel();
        expect(modelInstance.dismiss).toHaveBeenCalledWith("cancel");
    });
})