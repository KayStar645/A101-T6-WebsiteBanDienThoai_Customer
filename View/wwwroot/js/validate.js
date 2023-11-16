// const Validate = function (formId) {
//     const formElement = document.querySelector(formId);
//     const formRules = {};
//     const _this = this;
//     let isValid = false;

//     // Tìm phần tử cha của 1 thẻ input
//     const getParentElement = (input, parent) => {
//         // var parentElement = formElement.querySelector(parent);

//         while (input.parentElement) {
//             if (input.parentElement.matches(parent)) {
//                 return input.parentElement;
//             }

//             input = input.parentElement;
//         }
//     };

//     // Khởi tạo các rules cho các inputs
//     const validateRules = {
//         required: (value) => {
//             return value ? undefined : "Bạn phải nhập trường này";
//         },

//         fullname: (value) => {
//             var regex = /(^[A-Za-z\p{Ll}]{1,16})([ ]{0,1})([A-Za-z\p{Ll}]{1,16})?([ ]{0,1})?([A-Za-z\p{Ll}]{1,16})?([ ]{0,1})?([A-Za-z\p{Ll}]{1,16})/u;
//             return regex.test(value) ? undefined : "Vui lòng nhập chính xác họ tên của bạn";
//         },

//         email: (value) => {
//             var regex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
//             return regex.test(value) ? undefined : "Email không hợp lệ";
//         },

//         password: (value) => {
//             var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/;
//             return regex.test(value) ? undefined : "Mật khẩu từ 8 ký tự, ít nhất 1 ký tự là số, 1 ký tự in hoa";
//         },

//         phoneNumber: (value) => {
//             var regex = /^0[0-9]{9}$/;
//             return regex.test(value) ? undefined : "Số điện thoại không hợp lệ";
//         },

//         passwordConfirmation: (value) => {
//             var oldPass = formElement.querySelector("[name=password]");
//             return value === oldPass.value ? undefined : "Mật khẩu không khớp"
//         },
//     };

//     // Chỉ thực hiện khi tồn tại form
//     if (formElement) {
//         var inputs = formElement.querySelectorAll("[name][rule]");

//         inputs.forEach(input => {
//             var rule = input.getAttribute("rule");

//             // Lấy ra rules của từng input
//             if (input.getAttribute("rule").includes("|")) {
//                 rule = rule.split("|");
//             }
//             formRules[input.name] = rule;

//             // Gán từng rule trở thành 1 function trong validateRules
//             if (Array.isArray(formRules[input.name])) {
//                 for (var i = 0; i < formRules[input.name].length; i++) {
//                     var name = formRules[input.name][i];

//                     name = validateRules[name];
//                     formRules[input.name][i] = name;
//                 }
//             }
//             else {
//                 formRules[input.name] = [validateRules[rule]];
//             }
//             // Xử lý sự kiện trên từng input(blur, qinput)
//             input.addEventListener("blur", (e) => {
//                 onBlurEvent(e.target);
//             });

//             input.addEventListener("input", (e) => {
//                 onInputEvent(e.target);
//             });

//         });

//         // Xử lý sự kiện blur
//         function onBlurEvent(input) {
//             var rule = input.name;
//             var parentElement = getParentElement(input, ".form__group");
//             var error;

//             for (var i = 0; i < formRules[rule].length; i++) {
//                 var ruleTest = formRules[input.name][i];

//                 switch (input.type) {
//                     case "radio": {
//                         var radioCheck = formElement.querySelector("input[type=radio]:checked");

//                         error = ruleTest(radioCheck);
//                         break;
//                     }

//                     case "checkbox": {
//                         var checkBox = formElement.querySelector("input[type=checkbox]:checked");

//                         error = ruleTest(checkBox);
//                         break;
//                     }

//                     default: {
//                         error = ruleTest(input.value);
//                         break;
//                     }
//                 }

//                 if (error) break;
//             }

//             if (error) {
//                 parentElement.classList.add("form__group--invalid");
//                 parentElement.querySelector(".form__warning").innerText = error;
//             }

//             return !error;
//         }

//         // Xử lý sự kiện input
//         function onInputEvent(input) {
//             var parentElement = getParentElement(input, ".form__group");
//             parentElement.classList.remove("form__group--invalid");
//             parentElement.querySelector(".form__warning").innerText = "";
//         }
//     }

//     // Xử lý khi submit form
//     formElement.addEventListener('submit', (e) => {
//         e.preventDefault();

//         var inputs = formElement.querySelectorAll("[name][rule]");
//         var formInvalid;

//         inputs.forEach(input => {
//             formInvalid = onBlurEvent(input);
//         });

//         var formData = Array.from(inputs).reduce(function (value, input) {
//             switch (input.type) {
//                 case "radio": {
//                     var checked = formElement.querySelector("input[type=radio]:checked") ? formElement.querySelector("input[type=radio]:checked").value : null;
//                     checked ? value[input.name] = checked : undefined;
//                     break;
//                 }

//                 case "checkbox": {
//                     var checkboxs = formElement.querySelectorAll("input[type=checkbox]:checked");
//                     var checkboxValue = [];

//                     checkboxs.forEach(checkbox => {
//                         checkboxValue.push(checkbox.value);
//                     });

//                     checkboxValue.length > 0 ? value[input.name] = checkboxValue : undefined;
//                     break;
//                 }

//                 case "file": {
//                     input.value ? value[input.name] = input.files : undefined;
//                     break;
//                 }
//                 default:
//                     input.value ? value[input.name] = input.value : undefined;
//                     break;
//             }
//             return value;
//         }, {});

//         _this.onSubmit(formData);
//     });
// };
