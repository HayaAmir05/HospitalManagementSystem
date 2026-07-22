function togglePassword(inputId, btn) {
    const input = document.getElementById(inputId);
    if (input.type === "password") {
        input.type = "text";
        btn.textContent = "🙈";
    } else {
        input.type = "password";
        btn.textContent = "👁";
    }
}

// ===== Input restriction: digits only (Contact, phone-style fields) =====
function restrictToDigits(inputElement) {
    inputElement.value = inputElement.value.replace(/[^0-9]/g, '');
}

// ===== Input restriction: letters and spaces only (Name fields) =====
function restrictToLetters(inputElement) {
    inputElement.value = inputElement.value.replace(/[^a-zA-Z\s'-.]/g, '');
}

