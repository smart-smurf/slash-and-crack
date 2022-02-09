VERSION=1.0.2
release-it --only-version && \
  npx auto-changelog --commit-limit false -u --template keepachangelog && \
  cp CHANGELOG.md release_notes.txt && \
  git add CHANGELOG.md release_notes.txt && \
  git commit -m "chore(release): update changelog + release notes for v${VERSION}" && \
  git push
