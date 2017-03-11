describe('ErrorFormCtrl', function () {
    beforeEach(angular.mock.module("app"));
    beforeEach(angular.mock.module("appControllers"));

    let errorController: ErrorFormController;
    let modelInstance: angular.ui.bootstrap.IModalServiceInstance;
    const testTitle = "TestTitle1";
    const testMessage = "TestMessage1";

    beforeEach(inject(function ($controller: angular.IControllerService) {
        modelInstance = jasmine.createSpyObj('$uibModalInstance', ['close', 'dismiss']);

        errorController = $controller<ErrorFormController>('ErrorFormCtrl', {
            $uibModalInstance: modelInstance,
            title: testTitle,
            message: testMessage
        });
    }));

    it('should set the title and messages properties of scope to the provided values', function () {
        expect(errorController.title).toBe(testTitle);
        expect(errorController.message).toBe(testMessage);
    });

    it('should close the modal when the close binding is called', function () {
        errorController.close();

        expect(modelInstance.close).toHaveBeenCalled();
    });
})