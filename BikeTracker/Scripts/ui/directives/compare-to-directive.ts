interface IModelValidatorsWithCompareTo extends angular.IModelValidators {
    compareTo(modelValue: any): boolean;
}

interface ICompareToScope extends angular.IScope {
    otherModelValue: any;
}

class CompareToDirective {
    scope = {
        otherModelValue: "=compareTo"
    };

    link(scope: ICompareToScope, element: angular.IAugmentedJQuery, attributes: angular.IAttributes, ngModel: angular.INgModelController) {
        let val = ngModel.$validators as IModelValidatorsWithCompareTo;

        val.compareTo = function (modelValue) {
            return modelValue === scope.otherModelValue;
        };

        scope.$watch("otherModelValue", function () {
            ngModel.$validate();
        });
    }

    public static Factory() {
        var directive = () => {
            return new CompareToDirective();
        };

        return directive;
    }
}

app.directive("compareTo", CompareToDirective.Factory());