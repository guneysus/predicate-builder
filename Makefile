default: test

build:
	$(MAKE) -C src build

test:
	$(MAKE) -C src test

.PHONY: default build test
