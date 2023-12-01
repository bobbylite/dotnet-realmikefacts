// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('load', () => {
    PopupTour();
    BootstrapSpinnerAnimations();
});

function PopupTour(){ 
    const errorMessage = document.getElementsByClassName("text-danger");

    if (!errorMessage)
    {
        return;
    }

    if (errorMessage.length > 0)
    { 
        BeginAccessDeniedTour();
    }
}

function BeginAccessDeniedTour() {
    const tour = new Shepherd.Tour({
        useModalOverlay: true,
        defaultStepOptions: {
          classes: "shadow-md bg-purple-dark",
          scrollTo: true,
          scrollTo: {
            behavior: "smooth",
            block: "center",
          },
        },
      });
      
      const Intro = {
        id: "intro-step",
        text: "You can resolve <code>Access Denied</code> by creating an Access Request.",
        attachTo: {
          element: "",
          on: "center",
        },
        classes: "custom-tour",
        buttons: [
          {
            text: "Start tour",
            action: tour.next,
          },
        ],
      };
      
      const stepOne = {
        id: 'first-step',
        text: "Cause: C# attribute <code>[Authorize(Policy = PolicyNames.GroupPolicyName)]</code>. Go to your settings page to see group policies made available to you and request access to which ever ones you want. You will need <code>Beta testers</code> group policy for this page.",
        attachTo: {
            element: '.pb-3',
            on: 'bottom'
        },
        classes: 'custom-tour',
        buttons: [
            {
            text: 'Continue',
            action: tour.next
            }
        ]
        };
      
      const stepTwo = {
        id: "step2",
        text: "More information on Authorizing group policies here.",
        attachTo: {
          element: "path",
          on: "bottom",
        },
        classes: "custom-tour",
        buttons: [
          {
            text: "Complete",
            action: tour.next,
          },
        ],
      };
      
      const stepThree = {
        id: "step3",
        text: "Go to your settings page to see group policies made available to you and request access to which ever ones you want. You will need <code>Beta testers</code> group policy for this page.",
        attachTo: {
          element: ".popup-tour-settings",
          on: "bottom",
        },
        classes: "custom-tour",
        buttons: [
          {
            text: "Complete",
            action: tour.next,
          },
        ],
      };
      
      tour.addSteps([
        Intro,
        stepOne,
        stepTwo,
      ]);

      tour.start();
}

function BootstrapSpinnerAnimations() { 
    const button = document.getElementById("next");

    if (!button)
    { 
        return;
    }

    button.setAttribute("data-loading-text", '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Sign in');
    
    const form = button.closest("form");

    if (!form)
    { 
        return;
    }

    form.addEventListener("submit", function () {
        if (button.getAttribute("data-loading") === "true") {
            event.preventDefault();
        } else {
            button.disabled = true;
            button.setAttribute("data-loading", "true");
            button.innerHTML = button.getAttribute("data-loading-text");
            const spinner = button.querySelector('.spinner-border');
            spinner.classList.remove('d-none');
        }
    });

    const forgotPasswordButton = document.getElementById("forgotPassword");

    if (!forgotPasswordButton)
    { 
        return;
    }

    forgotPasswordButton.href = "https://realmikefacts.b2clogin.com/realmikefacts.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_realmikefacts_forgot_password&client_id=3d0989fb-80d7-42d9-b1e9-6e9528c105bc&nonce=defaultNonce&redirect_uri=https%3A%2F%2Frealmikefacts.azurewebsites.net%2Fsignin-oidc&scope=openid&response_type=id_token&prompt=login";
}