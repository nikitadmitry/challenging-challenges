/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

 CKEDITOR.editorConfig = function( config ) {
 	config.toolbarGroups = [
 		{ name: 'clipboard', groups: [ 'clipboard', 'undo' ] },
 		{ name: 'editing', groups: [ 'find', 'selection', 'spellchecker', 'editing' ] },
 		{ name: 'links', groups: [ 'links' ] },
 		{ name: 'insert', groups: [ 'insert' ] },
 		{ name: 'forms', groups: [ 'forms' ] },
 		{ name: 'tools', groups: [ 'tools' ] },
 		{ name: 'document', groups: [ 'mode', 'document', 'doctools' ] },
 		{ name: 'others', groups: [ 'others' ] },
 		'/',
 		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
 		{ name: 'paragraph', groups: [ 'list', 'indent', 'blocks', 'align', 'bidi', 'paragraph' ] },
 		{ name: 'styles', groups: [ 'styles' ] },
 		{ name: 'colors', groups: [ 'colors' ] },
 		{ name: 'about', groups: [ 'about' ] }
 	];

  config.removePlugins = 'spoiler';

  config.extraPlugins = 'uploadimage';
  config.extraPlugins = 'notification';

  config.filebrowserImageUploadUrl = '/Challenges/UploadFile';
	config.uploadUrl = '/Challenges/UploadFile';

 	config.removeButtons = 'Subscript,Superscript,SpellChecker';

  config.contentsCss = [ CKEDITOR.basePath + 'contents.css', 'http://sdk.ckeditor.com/samples/assets/css/widgetstyles.css' ];
	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

  config.extraPlugins = 'wordcount';
  config.wordcount = {
    showParagraphs: false,
    showWordCount: false,
    showCharCount: true,
    countHTML: true,
    maxCharCount: 1000
  };
};
