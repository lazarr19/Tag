	/* Data SHA1: 50e09cfaf0c0e64ab905ce936d4faecb4cca5e93 */
	.file	"typemap.jm.inc"

	/* Mapping header */
	.section	.data.jm_typemap,"aw",@progbits
	.type	jm_typemap_header, @object
	.p2align	2
	.global	jm_typemap_header
jm_typemap_header:
	/* version */
	.long	1
	/* entry-count */
	.long	95
	/* entry-length */
	.long	202
	/* value-offset */
	.long	85
	.size	jm_typemap_header, 16

	/* Mapping data */
	.type	jm_typemap, @object
	.global	jm_typemap
jm_typemap:
	.size	jm_typemap, 19191
	.include	"typemap.jm.inc"
