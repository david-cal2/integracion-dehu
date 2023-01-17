var psnn = new Object();

//general data
psnn.header = new Object();
psnn.header.class = '.gn-header';
psnn.header.selector = document.querySelector(psnn.header.class);
psnn.generalContent = document.querySelector('.general-layout');
psnn.content = document.querySelector('.body-main');

// bloquear pantalla o contenido
psnn.lock = new Object();
psnn.lock.content_class = 'contentBlock';
psnn.lock.window_class = 'winBlock';
psnn.lock.content_bodyClass = 'block-content';
psnn.lock.window_bodyClass = 'block-window';
psnn.lock.is_content = false;
psnn.lock.is_window = false;

var contentBlockDiv = document.createElement("div");
contentBlockDiv.className = psnn.lock.content_class;
var winBlockDiv = document.createElement("div");
winBlockDiv.className = psnn.lock.window_class;

//psnn.content.appendChild( contentBlockDiv );
//psnn.generalContent.appendChild( winBlockDiv );

psnn.lock.content = function (el) {
    document.body.classList.add(psnn.lock.content_bodyClass);
};
psnn.lock.window = function (el) {
    document.body.classList.add(psnn.lock.window_bodyClass);
};
psnn.lock.unlock = function (el) {
    if (document.body.classList.contains(psnn.lock.content_bodyClass)) {
        document.body.classList.remove(psnn.lock.content_bodyClass);
    }
    if (document.body.classList.contains(psnn.lock.window_bodyClass)) {
        document.body.classList.remove(psnn.lock.window_bodyClass);
    }
};
psnn.lock.unlockBg = function (bgEl) {
    if (bgEl != null) {
        bgEl.addEventListener('click', function () {
            if (document.body.classList.contains(psnn.lock.content_bodyClass) || document.body.classList.contains(psnn.lock.window_bodyClass)) {
                if (psnn.lat.selector.classList.contains(psnn.lat.expanded_class)) {
                    psnn.lat.selector.classList.toggle(psnn.lat.expanded_class);
                }
            }
            psnn.lock.unlock();
        }, false);
    }
};
psnn.lock.unlockBg(contentBlockDiv);
psnn.lock.unlockBg(psnn.header.selector);
psnn.lock.unlockBg(winBlockDiv);


// Loop del focus en un contenedor (modals, etc...)
psnn.tabLoop = new Object();
psnn.tabLoop.focusableElements = 'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])';
psnn.tabLoop.disable = false;

psnn.tabLoop.addTabLoop = function (elem) {

    const modal = document.querySelector(elem); // select the modal by it's id

    const firstFocusableElement = modal.querySelectorAll(psnn.tabLoop.focusableElements)[0]; // get first element to be focused inside modal
    const focusableContent = modal.querySelectorAll(psnn.tabLoop.focusableElements);
    const lastFocusableElement = focusableContent[focusableContent.length - 1]; // get last element to be focused inside modal


    document.addEventListener('keydown', function (e) {
        let isTabPressed = e.key === 'Tab' || e.keyCode === 9;

        if (!isTabPressed) {
            return;
        }

        if (e.shiftKey) { // if shift key pressed for shift + tab combination
            if (document.activeElement === firstFocusableElement) {
                lastFocusableElement.focus(); // add focus for the last focusable element
                e.preventDefault();
            }
        } else { // if tab key is pressed
            if (document.activeElement === lastFocusableElement) { // if focused has reached to last focusable element then focus first focusable element after pressing tab
                firstFocusableElement.focus(); // add focus for the first focusable element
                e.preventDefault();
            }
        }
    });

};



// lateral stiky scroll
psnn.lat = new Object();
psnn.lat.is_lateralScroll_class = '.body-aside';
psnn.lat.is_fix_class = 'fix-lateral';
psnn.lat.selector = document.querySelector(psnn.lat.is_lateralScroll_class);

if (typeof psnn.lat.selector != "undefined") {
    var wrap = psnn.lat.selector;
    if (wrap != null) {
        psnn.lat.setScrollbar = function (param) {
            if (param.target.scrollingElement.scrollTop > psnn.header.selector.clientHeight) {
                wrap.classList.add(psnn.lat.is_fix_class);
            } else {
                wrap.classList.remove(psnn.lat.is_fix_class);
            }
        };
        window.addEventListener("scroll", function (e) {
            psnn.lat.setScrollbar(e);
        });
    }
}

// lateral menu
// psnn.lat.hamBtn = 'lateral-nav';
// psnn.lat.expanded_class = 'expanded';
// psnn.lat.hamBtn_selector = document.getElementsByClassName(psnn.lat.hamBtn);

// for (var i = 0; i < psnn.lat.hamBtn_selector.length; i++) {
//     psnn.lat.hamBtn_selector[i].addEventListener('click', function () {
//         psnn.lat.selector.classList.toggle(psnn.lat.expanded_class);
//         if(psnn.lat.selector.classList.contains(psnn.lat.expanded_class)){
//             psnn.lock.content();
//             psnn.tabLoop.addTabLoop('.body-aside.expanded .lateral-popup');
//             //psnn.lock.window();
//         }else{
//             psnn.lock.unlock();
//             document.querySelector('.body-aside .aside .lateral-nav').focus();  
//         }
//     }, false);
// }

// Submenú usuario
psnn.userNav = new Object();
psnn.userNav.menuItems = document.querySelectorAll('li.has-submenu');
Array.prototype.forEach.call(psnn.userNav.menuItems, function (el, i) {
    //var activatingA = el.querySelector('button');
    //var btn = '<button><span><span class="visuallyhidden">show submenu for “' + activatingA.text + '”</span></span></button>';
    //activatingA.insertAdjacentHTML('afterend', btn);

    var openCloseText;
    el.querySelector('button').addEventListener("click", function (event) {
        if (this.parentNode.classList.contains("open")) {
            this.parentNode.classList.remove("open");
            // this.parentNode.querySelector('a, span').setAttribute('aria-expanded', "false");
            this.parentNode.querySelector('button').setAttribute('aria-expanded', "false");
            openCloseText = this.getAttribute('data-open');
            this.querySelector('span').innerHTML = openCloseText;
            console.log(openCloseText);
        } else {
            this.parentNode.classList.add("open");
            // this.parentNode.querySelector('a, span').setAttribute('aria-expanded', "true");
            this.parentNode.querySelector('button').setAttribute('aria-expanded', "true");
            openCloseText = this.getAttribute('data-close');
            this.querySelector('span').innerHTML = openCloseText;
        }
        event.preventDefault();
    });

});
document.body.addEventListener("click", function (e) {
    var element = e.target;
    var parent = document.querySelector('li.has-submenu.open');
    if (parent !== null) {
        if (!parent.contains(element)) {
            parent.className = "has-submenu";
            // parent.querySelector('a,span').setAttribute('aria-expanded', "false");
            parent.querySelector('button').setAttribute('aria-expanded', "false");
            var openCloseText = parent.querySelector('button').getAttribute('data-open');
            parent.querySelector('button span').innerHTML = openCloseText;
        }
    }
});


