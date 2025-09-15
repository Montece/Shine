// Shine Web Client - Main JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Auto-hide alerts
    setTimeout(function() {
        const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
        alerts.forEach(function(alert) {
            const bsAlert = new bootstrap.Alert(alert);
            if (bsAlert) {
                bsAlert.close();
            }
        });
    }, 5000);

    // Form validation enhancement
    const forms = document.querySelectorAll('form');
    forms.forEach(function(form) {
        form.addEventListener('submit', function(event) {
            const submitButton = form.querySelector('button[type="submit"]');
            if (submitButton && form.checkValidity()) {
                // Add loading state
                submitButton.classList.add('loading');
                submitButton.disabled = true;
                
                // Store original text
                const originalText = submitButton.innerHTML;
                submitButton.dataset.originalText = originalText;
                
                // Add loading text
                const loadingText = submitButton.dataset.loadingText || 'Загрузка...';
                submitButton.innerHTML = `<i class="fas fa-spinner fa-spin me-2"></i>${loadingText}`;
                
                // Reset after 10 seconds (safety)
                setTimeout(function() {
                    if (submitButton.classList.contains('loading')) {
                        submitButton.classList.remove('loading');
                        submitButton.disabled = false;
                        submitButton.innerHTML = submitButton.dataset.originalText || originalText;
                    }
                }, 10000);
            }
        });
    });

    // Auto-focus modal inputs
    const modals = document.querySelectorAll('.modal');
    modals.forEach(function(modal) {
        modal.addEventListener('shown.bs.modal', function() {
            const firstInput = modal.querySelector('input:not([type="hidden"]):not(:disabled)');
            if (firstInput) {
                firstInput.focus();
            }
        });

        modal.addEventListener('hidden.bs.modal', function() {
            // Clear form data when modal is closed
            const form = modal.querySelector('form');
            if (form) {
                const inputs = form.querySelectorAll('input:not([type="hidden"]):not([type="submit"])');
                inputs.forEach(function(input) {
                    if (input.type !== 'checkbox' && input.type !== 'radio') {
                        input.value = '';
                    }
                });
                
                // Clear validation errors
                const errorElements = form.querySelectorAll('.text-danger');
                errorElements.forEach(function(error) {
                    error.textContent = '';
                });
                
                // Remove validation classes
                const validatedElements = form.querySelectorAll('.is-valid, .is-invalid');
                validatedElements.forEach(function(element) {
                    element.classList.remove('is-valid', 'is-invalid');
                });
            }
        });
    });

    // Smooth scrolling for anchor links
    const anchorLinks = document.querySelectorAll('a[href^="#"]');
    anchorLinks.forEach(function(link) {
        link.addEventListener('click', function(event) {
            const targetId = this.getAttribute('href').substring(1);
            const targetElement = document.getElementById(targetId);
            
            if (targetElement) {
                event.preventDefault();
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Enhanced search functionality
    const searchInputs = document.querySelectorAll('input[type="search"], input[name*="search" i]');
    searchInputs.forEach(function(input) {
        let searchTimeout;
        
        input.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(function() {
                // Auto-submit search form after 500ms of inactivity
                const form = input.closest('form');
                if (form && input.value.length > 2) {
                    form.submit();
                }
            }, 500);
        });
    });

    // Progress bar animations
    const progressBars = document.querySelectorAll('.progress-bar');
    progressBars.forEach(function(bar) {
        const width = bar.style.width;
        bar.style.width = '0%';
        
        setTimeout(function() {
            bar.style.transition = 'width 1s ease-in-out';
            bar.style.width = width;
        }, 100);
    });

    // Card hover effects
    const cards = document.querySelectorAll('.card');
    cards.forEach(function(card) {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-2px)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });

    // Confirmation dialogs
    const confirmButtons = document.querySelectorAll('[data-confirm]');
    confirmButtons.forEach(function(button) {
        button.addEventListener('click', function(event) {
            const message = this.dataset.confirm;
            if (!confirm(message)) {
                event.preventDefault();
                return false;
            }
        });
    });

    // Copy to clipboard functionality
    const copyButtons = document.querySelectorAll('[data-copy]');
    copyButtons.forEach(function(button) {
        button.addEventListener('click', function() {
            const textToCopy = this.dataset.copy;
            navigator.clipboard.writeText(textToCopy).then(function() {
                // Show success message
                showToast('Скопировано в буфер обмена!', 'success');
            }).catch(function() {
                // Fallback for older browsers
                const textArea = document.createElement('textarea');
                textArea.value = textToCopy;
                document.body.appendChild(textArea);
                textArea.select();
                document.execCommand('copy');
                document.body.removeChild(textArea);
                showToast('Скопировано в буфер обмена!', 'success');
            });
        });
    });
});

// Utility functions
function showToast(message, type = 'info') {
    // Create toast element
    const toastContainer = document.getElementById('toast-container') || createToastContainer();
    
    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type} border-0`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;
    
    toastContainer.appendChild(toast);
    
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();
    
    // Remove toast element after it's hidden
    toast.addEventListener('hidden.bs.toast', function() {
        toast.remove();
    });
}

function createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toast-container';
    container.className = 'toast-container position-fixed top-0 end-0 p-3';
    container.style.zIndex = '1055';
    document.body.appendChild(container);
    return container;
}

function showLoading(element) {
    element.classList.add('loading');
    element.disabled = true;
}

function hideLoading(element) {
    element.classList.remove('loading');
    element.disabled = false;
}

// Global error handler
window.addEventListener('error', function(event) {
    console.error('Global error:', event.error);
    showToast('Произошла ошибка. Пожалуйста, попробуйте еще раз.', 'danger');
});

// Service worker registration (if needed in the future)
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
        // navigator.serviceWorker.register('/sw.js');
    });
}
