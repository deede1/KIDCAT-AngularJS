require.config({

    baseUrl: "/mvc/Areas/Mobile/App/",
    
	// alias libraries paths
    paths: {
        'angular': '../Public/lib/angularjs/angular',
        'angular-route': '../Public/lib/angularjs/angular-route.min',
        'angular-cookie': '../Public/lib/angularjs/angular-cookies.min',
        'angular-animate': '../Public/lib/angularjs/angular-animate',
        'angular-sanitize': '../Public/lib/angularjs/angular-sanitize.min',
        'async': '../Public/lib/requirejs/async',
        'angularAMD': '../Public/lib/angularjsAMD/angularAMD',
        'ngload': '../Public/lib/angularjsAMD/ngload',
        'bootstrap': '../Public/lib/boostrap/js/bootstrap',
        'ui-bootstrap': '../Public/lib/angularjsUI/ui-bootstrap-tpls-0.11.0.min',        
        'prettify': '../Public/lib/google-code-prettify/prettify',
        'xml2json': '../Public/lib/jquery/xml2json',
        'jquery': '../Public/lib/jquery/jquery-2.1.1.min'

    },

    // Add angular modules that does not support AMD out of the box, put it in a shim
    shim: {
        'angularAMD': ['angular'],
        'angular-route': ['angular'],
        'angular-cookie': ['angular'],
        'angular-animate': ['angular'],
        'angular-sanitize': ['angular'],
        'ui-bootstrap': ['angular']
    },

    // kick start application
    deps: ['app']
});
