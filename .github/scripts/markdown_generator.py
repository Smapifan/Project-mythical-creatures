import os
import json

output_dir = "Results"
lines = ["## 🗂️ File Types (alle Branches)\n"]

# Alle JSON-Dateien verarbeiten
for filename in sorted(os.listdir(output_dir)):
    if not filename.endswith(".json"):
        continue
    path = os.path.join(output_dir, filename)
    with open(path, encoding="utf-8") as f:
        data = json.load(f)

    branch = data["branch"]
    filetypes = data["filetypes"]
    total = data["total_files"]

    lines.append(f"### Branch `{branch}`")
    lines.append("| File Type | Count | Percentage |")
    lines.append("|-----------|-------|------------|")

    for ext, count in sorted(filetypes.items(), key=lambda x: -x[1]):
        percent = f"{(count/total)*100:.2f}%"
        lines.append(f"| `{ext}` | {count} | {percent} |")

    lines.append("")

# Markdown speichern
with open("FILETYPES.md", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))
