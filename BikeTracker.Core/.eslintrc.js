module.exports = {
    "extends": "default",
    "parserOptions": {
        "ecmaVersion": 6,
        "sourceType": "module",
        "ecmaFeatures": {
            "jsx": true
        }
    },
    "plugins": [
        "react"
    ],
    "rules": {
        "react/jsx-uses-react": "error",
        "react/jsx-uses-vars": "error",
        "indent": ["error", 4],
        "space-before-function-paren": ["error", "never"],
        "semi": ["error", "always"]
    }
};
