# HANDOVER_NanoImpactNET_20261117.md -> 인수인계문서_NanoImpactNET_20261117.docx
# 사용: 이 폴더에서  py make_docx.py
# 지원하는 md: # 제목, 표(|), 코드 블록(```), 글머리(- / 1.), **굵게**, `코드`, ---(구분선), 일반 문단
import os
import re
from docx import Document
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Pt, RGBColor

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)
SRC = os.path.join(ROOT, "HANDOVER_NanoImpactNET_20261117.md")
DST = os.path.join(ROOT, "인수인계문서_NanoImpactNET_20261117.docx")
FONT = "맑은 고딕"
MONO = "Consolas"


def set_font(run, name=FONT, size=None, bold=None, color=None):
    run.font.name = name
    rpr = run._element.get_or_add_rPr()
    fonts = rpr.find(qn("w:rFonts"))
    if fonts is None:
        fonts = OxmlElement("w:rFonts")
        rpr.append(fonts)
    fonts.set(qn("w:eastAsia"), FONT)
    if size:
        run.font.size = Pt(size)
    if bold is not None:
        run.bold = bold
    if color:
        run.font.color.rgb = color


def add_inline(par, text, size=None):
    # **굵게**, `코드` 처리
    for part in re.split(r"(\*\*[^*]+\*\*|`[^`]+`)", text):
        if not part:
            continue
        if part.startswith("**") and part.endswith("**"):
            set_font(par.add_run(part[2:-2]), size=size, bold=True)
        elif part.startswith("`") and part.endswith("`"):
            set_font(par.add_run(part[1:-1]), name=MONO, size=(size or 10.5) - 1, color=RGBColor(0x8B, 0x1A, 0x1A))
        else:
            set_font(par.add_run(part), size=size)


def shade(cell, hex_color):
    tcpr = cell._element.get_or_add_tcPr()
    shd = OxmlElement("w:shd")
    shd.set(qn("w:val"), "clear")
    shd.set(qn("w:color"), "auto")
    shd.set(qn("w:fill"), hex_color)
    tcpr.append(shd)


def add_table(doc, rows):
    cells = [[c.strip() for c in r.strip().strip("|").split("|")] for r in rows]
    cells = [r for r in cells if not all(re.fullmatch(r":?-{3,}:?", c) for c in r)]
    ncol = max(len(r) for r in cells)
    t = doc.add_table(rows=len(cells), cols=ncol)
    t.style = "Table Grid"
    t.alignment = WD_TABLE_ALIGNMENT.LEFT
    for i, r in enumerate(cells):
        for j in range(ncol):
            cell = t.cell(i, j)
            cell.paragraphs[0].text = ""
            add_inline(cell.paragraphs[0], r[j] if j < len(r) else "", size=9)
            if i == 0:
                for run in cell.paragraphs[0].runs:
                    run.bold = True
                shade(cell, "DCE6F0")
    doc.add_paragraph()


def main():
    doc = Document()
    st = doc.styles["Normal"]
    st.font.name = FONT
    st.font.size = Pt(10.5)
    st.element.rPr.rFonts.set(qn("w:eastAsia"), FONT)

    lines = open(SRC, encoding="utf-8").read().splitlines()
    i = 0
    while i < len(lines):
        line = lines[i]
        s = line.strip()
        if s.startswith("```"):
            i += 1
            code = []
            while i < len(lines) and not lines[i].strip().startswith("```"):
                code.append(lines[i])
                i += 1
            p = doc.add_paragraph()
            set_font(p.add_run("\n".join(code)), name=MONO, size=8.5)
            p.paragraph_format.left_indent = Pt(12)
            i += 1
            continue
        if s.startswith("|"):
            rows = []
            while i < len(lines) and lines[i].strip().startswith("|"):
                rows.append(lines[i])
                i += 1
            add_table(doc, rows)
            continue
        m = re.match(r"^(#{1,4})\s+(.*)$", s)
        if m:
            level = len(m.group(1))
            h = doc.add_heading(level=level - 1 if level > 1 else 0)
            add_inline(h, m.group(2))
            for run in h.runs:
                set_font(run, size={1: 18, 2: 14, 3: 12, 4: 11}[level], bold=True, color=RGBColor(0x1F, 0x3A, 0x5F))
        elif s == "---":
            doc.add_paragraph("─" * 40)
        elif re.match(r"^\s*-\s+", line):
            indent = (len(line) - len(line.lstrip())) // 2
            p = doc.add_paragraph(style="List Bullet" if indent == 0 else "List Bullet 2")
            add_inline(p, re.sub(r"^\s*-\s+", "", line))
        elif re.match(r"^\s*\d+\.\s+", line):
            indent = (len(line) - len(line.lstrip())) // 2
            p = doc.add_paragraph(style="List Number" if indent == 0 else "List Number 2")
            add_inline(p, re.sub(r"^\s*\d+\.\s+", "", line))
        elif re.match(r"^\s+\S", line) and doc.paragraphs and doc.paragraphs[-1].style.name.startswith("List"):
            # 글머리 항목이 다음 줄로 이어지는 경우
            add_inline(doc.paragraphs[-1], " " + s)
        elif s:
            add_inline(doc.add_paragraph(), s)
        i += 1
    doc.save(DST)
    print("saved", DST)


if __name__ == "__main__":
    main()
