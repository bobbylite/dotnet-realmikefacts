// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('load', () => {
    PopupTour();
    BootstrapSpinnerAnimations();
});

function PopupTour(){ 
    const isErrorPage = document.getElementsByClassName("text-danger");

    if (!isErrorPage)
    {
        return;
    }

    if (isErrorPage.length > 0)
    { 
        BeginAccessDeniedTour();
    }

    const isSettingsPage = document.getElementsByClassName("popup-groups-tour");

    if (!isSettingsPage)
    {
        return;
    }

    if (isSettingsPage.length > 0)
    { 
        BeginAccessRequestTour();
    }
}

function BeginAccessRequestTour() {
      if (getQueryVariable('shepherdTour') != false){
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

        const stepOne = {
            id: 'stepOne',
            text: "Here's where you can find group policies and create <code>Acess Requests</code>.",
            attachTo: {
                element: 'tbody',
                on: 'top'
            },
            classes: 'custom-tour',
            buttons: [
                {
                    text: 'Skip',
                    action: () => {
                        tour.complete();
                    },
                    secondary: true
                },
                {
                    text: 'Continue',
                    action: tour.next
                }
            ]
        };

        const stepTwo = {
            id: 'stepTwo',
            text: "Here's how to make an Access Request for the <code>Beta tester</code> group policy. A RealMikeFacts administrator will approve or deny your request.",
            attachTo: {
                element: '.popup-groups-tour',
                on: 'bottom'
            },
            classes: 'custom-tour',
            buttons: [
                {
                    text: 'Finish',
                    action: () => {
                        tour.complete();
                    }
                }
            ]
        };

        tour.addSteps([
            stepOne,
            stepTwo
        ]);

        tour.start();
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
      
    const stepOne = {
        id: 'first-step',
        text: "This means that your identity in active directory is not a member of the <code>Beta tester</code> group policy needed to access this resource. Would you like a tour of how to resolve this with an <code>Access Request</code>?",
        attachTo: {
            element: '.pb-3',
            on: 'bottom'
        },
        classes: 'custom-tour',
        buttons: [
            {
                text: 'Skip',
                action: () => {
                    tour.complete();
                },
                secondary: true
            },
            {
                text: 'Start',
                action: tour.next
            }
        ]
    };
      
    const stepTwo = {
        id: "step2",
        text: "Check out our Github for more information on authorizing group policies with Azure Ad.",
        attachTo: {
          element: "path",
          on: "bottom",
        },
        classes: "custom-tour",
        buttons: [
          {
            text: "Continue",
            action: tour.next,
          },
        ],
    };

    tour.addSteps([
        stepOne,
        stepTwo
    ]);

    if(/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) || document.body.clientWidth < 600) {
        let stepThree = {
            id: "step3",
            text: () => {
                if ($(".navbar-collapse").hasClass("collapse")) {
                    $(".navbar-collapse").slideDown();
                }
                else {
                    $(".navbar-collapse").hide();
                }
                return "Go to your settings page to see group policies made available to you and request access to which ever ones you want. You will need <code>Beta testers</code> group policy for this page.";
            },
            attachTo: {
                element: '#settings-icon',
                on: "bottom",
            },
            classes: "custom-tour",
            buttons: [
                {
                    text: "Finish",
                    action: () => {
                        tour.complete();
                        go_home("step3", "AccessDeniedTour");
                    },
                    secondary: true
                },
                {
                    text: "Take me there",
                    action: tour.next
                },
            ],
        };

        tour.addSteps([
            stepThree
        ]);
    }
    else {
        let stepThree = {
            id: "step3",
            text: "Go to your settings page to see group policies made available to you and request access to which ever ones you want. You will need <code>Beta testers</code> group policy for this page.",
            attachTo: {
                element: '#settings-icon',
                on: "bottom",
            },
            classes: "custom-tour",
            buttons: [
                {
                    text: "Finish",
                    action: () => {
                        continue_tour_on_next_page("/", "step3", "AccessDeniedTour");
                        tour.complete();
                    },
                    secondary: true
                },
                {
                    text: "Take me there",
                    action: tour.next
                },
            ],
        };

        tour.addSteps([
            stepThree
        ]);
    }

    let stepFour = {
        id: "step4",
        text: () => {
            $(".navbar-collapse").slideUp();
            continue_tour_on_next_page("Settings", "step4", "AccessDeniedTour")
        },
        attachTo: {
            element: 'html',
            on: "right",
        },
        classes: "custom-tour",
        when: {
            show: () => {
                tour.complete();
            }
        }
    };

    tour.addSteps([
        stepFour
    ]);

    tour.start();
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    return false
}

function go_home(step, tour=Shepherd.activeTour.options.id){
    var new_route = `/?shepherdTour=${tour}&shepherdStep=${step}`
    window.location.replace(new_route);
}

function continue_tour_on_next_page(route, step, tour=Shepherd.activeTour.options.id){
    var new_route = `/${route}?shepherdTour=${tour}&shepherdStep=${step}`
    window.location.replace(new_route);
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